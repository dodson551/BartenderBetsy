Imports System
Imports System.IO
Imports System.IO.Compression
Imports System.Net.Sockets
Imports System.Text
Imports Microsoft.Win32
Imports System.Threading
Imports System.Net
Imports System.Collections
Imports System.Diagnostics
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports System.Reflection.Assembly
Imports System.Resources
Imports System.Reflection

'This is the base server class that all other server clasess use (Port 1001 Default)
Public Class HTTPServer

  Public DownloadPath As String

  Public isRunning As Boolean
  Public LocalPort As Integer
  Public ClientList As List(Of ThreadedClient)
  Public listener As TcpListener
  Public ServerThread As Threading.Thread

  Public Delegate Function ProcessWebRequestDelegate(ByRef hd As HttpData) As Boolean
  Private ProcessWebRequest As ProcessWebRequestDelegate = Nothing

  Public Sub New(ByVal CommunicationPort As Integer, pwr As ProcessWebRequestDelegate)
    LocalPort = CommunicationPort
    ProcessWebRequest = pwr
  End Sub

  Public Structure EventInfo
    'Other Variables
    Dim sMessage As String
    Dim dTimeStamp As DateTime
    Dim sActivity As String
  End Structure

  Public Event HttpConnected(ByVal sender As ThreadedClient)
  Public Event HttpInfo(ByVal sActivity As String)
  Public Event HttpError(ByVal sError As String)
  Public Event HttpDataRecv(ByVal sender As ThreadedClient, ByVal Data As String)
  Public Event HttpDataSent(ByVal sender As ThreadedClient, ByVal Data As String)

  Public Event ServerError(ByVal sError As String)
  Public Event StatusUpdate(ByVal sError As String)

  Public Event HttpDisconnected(ByVal sender As ThreadedClient)

  '===============================================================================
  ' Name:    Sub Wait
  ' Input:   ByVal sMilliseconds As Single
  ' Purpose: allows events 
  '===============================================================================
  Private Sub Wait(ByVal sMilliseconds As Single)
    Debug.WriteLine("Wait")
    Dim st As New Stopwatch
    st.Start()
    Do
      Application.DoEvents()
    Loop While st.ElapsedMilliseconds < sMilliseconds
    st.Stop()
  End Sub

  Public Sub StartServer()
    Debug.WriteLine("StartServer")
    'start a new thread for the server to live on
    isRunning = True
    ServerThread = New Threading.Thread(AddressOf ListenForConnections)
    ServerThread.Name = "Server Thread"
    ServerThread.Start()
  End Sub

  Public Sub ReStartServer()
    Debug.WriteLine("ReStartServer")
    Try
      isRunning = False
      listener.Stop()

      If ClientList IsNot Nothing Then
        For Each cClient As ThreadedClient In ClientList
          CloseClient(cClient)
        Next
      End If

      ServerThread.Abort()
      If ClientList IsNot Nothing Then ClientList.Clear()

      'start a new thread for the server to live on
      ServerThread = New Threading.Thread(AddressOf ListenForConnections)
      ServerThread.Name = "Server Thread"
      ServerThread.Start()
      RaiseEvent HttpInfo("Server Restarted")
      isRunning = True

    Catch ex As Exception
      MsgBox("RestartServer : " & ex.Message)
    End Try

  End Sub

  Public Sub StopServer()
    Debug.WriteLine("StopServer")
    Try
      If listener IsNot Nothing Then
        listener.Stop()
      End If
    Catch ex As System.Net.Sockets.SocketException
      Debug.Print("Thrown Error in StopServer (listener.Stop): " & ex.Message.ToString)
    Finally
      listener = Nothing
    End Try

    Try
      If ClientList IsNot Nothing Then
        For Each cClient As ThreadedClient In ClientList
          CloseClient(cClient)
        Next
      End If
    Catch ex As System.Net.Sockets.SocketException
      Debug.Print("Thrown Error in StopServer (CloseClient(cClient)): " & ex.Message.ToString)
    Finally
      ClientList = Nothing
    End Try

    GC.Collect()

    RaiseEvent HttpInfo("HttpServer stopped")
  End Sub

  Public Sub CloseClient(ByRef cClient As ThreadedClient)
    Debug.WriteLine("CloseClient")
    Try
      Dim ns As NetworkStream = cClient.TcpClient.GetStream
      cClient.Connected = False
      cClient.TcpClient.Close()
      ns.Flush()
      ns.Close()
      cClient.ClientThread.Abort()
      ClientList.Remove(cClient)
      RaiseEvent HttpDisconnected(cClient)
    Catch ex As Exception
      Debug.Print("CloseClient returned error: " & ex.Message)
    End Try
  End Sub

  Public Sub ListenForConnections()

    Debug.WriteLine("ListenForConnections")
    RaiseEvent HttpInfo("Server starting on " & LocalPort)
    Try
      'create the server socket
      listener = New TcpListener(System.Net.IPAddress.Any, LocalPort)
      listener.Start()
      RaiseEvent HttpInfo("Server started on " & LocalPort)
      Do Until False
        Thread.Sleep(100) 'let the thread rest

        'Dim cClient As New ThreadedClient(Me.listener.AcceptTcpClient, Me)
        If (Me.listener Is Nothing) Then Exit Do
        Dim cClient As New ThreadedClient(Me.listener.AcceptTcpClient)
        AddHandler cClient.ClientConnect, AddressOf OnClientConnect
        AddHandler cClient.ClientClose, AddressOf OnClientDisconnect
        AddHandler cClient.FirstPacketReceived, AddressOf OnFirstPacketReceived

        If ClientList Is Nothing Then ClientList = New List(Of ThreadedClient)
        ClientList.Add(cClient)
        cClient.ClientThread.Start()
        'Debug.Print("ClientList Count = " & ClientList.Count)
        'Sleep all clients
      Loop

    Catch ex As Exception
      isRunning = False
      RaiseEvent HttpInfo("Server FAILED on " & LocalPort)
    End Try

  End Sub

  Public Sub OnFirstPacketReceived(ByVal tc As ThreadedClient, ByVal baRecvBytes() As Byte)
    'Debug.WriteLine("OnFirstPacketReceived")
    Dim hd As New HttpData
    ' Dim sHeaderData As String = Encoding.ASCII.GetString(baRecvBytes)
    Dim aucReadBuff(0 To CInt(tc.TcpClient.ReceiveBufferSize) - 1) As Byte
    Dim iBytesRead As Integer
    Dim networkStream As NetworkStream = tc.TcpClient.GetStream()

    'get the request and send the data back to them
    Try
      ' TODO : in the future we may want to process multiple packets here while the socket is open...
      Dim sIPAddress As String = "", iPort As Integer = 0
      If GetIPandPort(tc.TcpClient, sIPAddress, iPort) Then
        hd.Request_RemoteIP = sIPAddress
        hd.Request_RemotePort = iPort
      End If

      hd.ParseHttpRequest(baRecvBytes, True)

      If hd.Request_Method = "POST" Then
        Debug.Print(hd.Response_BodyAscii) ' THIS WAS ONLY FOR DEBUG PURPOSES
      End If
      ' Get the rest of the packets for this message
      While Not (hd.Request_ContentComplete)
        networkStream.ReadTimeout = 5000
        iBytesRead = networkStream.Read(aucReadBuff, 0, CInt(tc.TcpClient.ReceiveBufferSize))
        Dim aucNext(0 To iBytesRead - 1) As Byte
        Array.Copy(aucReadBuff, aucNext, iBytesRead)
        hd.ParseHttpRequest(aucNext, False)
      End While

      ' hd now contains header information and data
      Dim sPortInfo As String = "Remote IP " & hd.Request_RemoteIP &
                   " : Port " & hd.Request_RemotePort & vbCrLf
      RaiseEvent HttpDataRecv(tc, sPortInfo & hd.Request_Head & hd.Response_BodyAscii)
      ServeHTTPData(tc, hd)
      If hd.Response_ContentType.Contains("text") Then
        RaiseEvent HttpDataSent(tc, hd.Request_Head & hd.Response_Head & hd.Response_BodyAscii)
      Else
        RaiseEvent HttpDataSent(tc, hd.Request_Head & hd.Response_Head & hd.Response_ContentType & vbCrLf & "Server Generated Request ID:" & hd.Request_ID)
      End If

    Catch ex As Exception
      RaiseEvent HttpError("OnFirstPacketReceived :" & ex.Message)
    Finally
      tc.CloseClienHttpConn()
    End Try
  End Sub

  Public Function GetIPandPort(ByVal tc As TcpClient,
                 ByRef sIPAddress As String,
                 ByVal iPort As Integer) As Boolean
    Dim bRet As Boolean = False
    Try
      ' Get the clients IP address using Client property            
      Dim ipEndPoint As Net.IPEndPoint = tc.Client.RemoteEndPoint
      If Not ipEndPoint Is Nothing Then
        sIPAddress = ipEndPoint.Address.ToString
        iPort = ipEndPoint.Port
        bRet = True
      End If
    Catch ex As System.ObjectDisposedException
      sIPAddress = String.Empty
    Catch ex As SocketException
      sIPAddress = String.Empty
    End Try
    Return bRet
  End Function

  Public Sub OnClientConnect(ByVal tc As ThreadedClient)
    RaiseEvent HttpConnected(tc)
  End Sub

  Public Sub OnClientDisconnect(ByVal tc As ThreadedClient)
    RaiseEvent HttpDisconnected(tc)
    On Error Resume Next
    tc.Connected = False
    tc.TcpClient.Close()
    ClientList.Remove(tc)
    tc = Nothing
  End Sub

  Protected Overrides Sub Finalize()
    StopServer()
    ClientList = Nothing
    listener = Nothing
    MyBase.Finalize()
  End Sub

  Private m_sHeader As String = "<!doctype html><html><head></head><body>" & vbCrLf
  Private m_sFooter As String = vbCrLf & "</body></html>"

  Private Sub ServeHTTPData(ByVal tc As ThreadedClient, ByRef hd As HttpData)
    Dim TrimChars() As Char = {"/", " ", "\"}
    Dim sOption As String
    Dim sBody As String = ""
    Dim sClientEndpoint As String = tc.TcpClient.Client.RemoteEndPoint.ToString.Split(":")(0)
    Dim sRootURL As String = hd.Request_Filename

    Dim iPos As Integer = 0
    Dim aucHeaders As Byte()

    ' HttpConn Variables
    hd.Response_Number = HttpData.HttpResponseCodes.HTTP_OK
    hd.Response_Text = "OK"
    hd.Response_ContentType = "text/html"

    Dim trim_char As Char() = {"/", "&", "?"}
    iPos = sRootURL.IndexOf("?", 1) ' First look and eliminate any "?" chars
    If iPos > 1 Then
      sRootURL = sRootURL.Substring(0, iPos).TrimEnd(trim_char) ' remove / if it is in URL
    End If

    iPos = sRootURL.IndexOf("&", 1) ' Then look and eliminate any "&" chars
    If iPos > 1 Then
      sRootURL = sRootURL.Substring(0, iPos).TrimEnd(trim_char) ' remove / if it is in URL
    End If

    sOption = sRootURL.ToLower.Trim(TrimChars)
    If sOption.Contains("download/") And sOption.Length > ("download/").Length Then
      sOption = "file"
    End If

    Try

      Select Case sOption
        Case ""
          sBody += "Server alive : " & Format(Now, "yyyy-MM-dd HH:mm:ss") & "<br>" & vbCrLf
          If Not (Me.DownloadPath Is Nothing) AndAlso Me.DownloadPath.Trim <> "" Then
            sBody += "<br /><p><a href=""/download"">" &
             "Click Here for Open Download Directory</a></p>"
          End If
          'hd.Response_Body = StringToByteArray(sBody)
          hd.Response_Body = ASCIIEncoding.ASCII.GetBytes(sBody)

        Case "exit"
          sBody = "<!doctype html><html><head></head><body>"

          sBody += "<font color=red>" & "Goodbye!" & "</font><br /><br />"

        Case "download"
          sBody += "<h2>Download Directory is: #</h2>".Replace("#", DownloadPath)
          sBody += "<ul>"
          For Each myFile As String In Directory.GetFiles(Me.DownloadPath)
            Dim sShortFile As String = myFile.Replace(DownloadPath & "\", "")
            sBody += "<li><a href=""/download/" & sShortFile & """>" & sShortFile & "</a></li>" & vbCrLf
          Next
          sBody += "</ul></body></html>" & vbCrLf
          hd.Response_Body = ASCIIEncoding.ASCII.GetBytes(sBody)
          hd.Response_Number = HttpData.HttpResponseCodes.HTTP_OK

          'Case "favicon.ico"
          '  hd.Response_ContentType = "image/x-icon"
          '  hd.Response_Body = ConvertIconToByteArray(My.Resources.FAVICON)
          '  hd.Response_ContentLength = hd.Response_Body.Length

        Case "file"
          Dim sShortFile As String = sRootURL.Replace("/download/", "").Replace("%20", " ")
          Dim sLongFile As String = (DownloadPath & "\" & sShortFile).Replace("\\", "\")
          Dim ba As Byte() = Nothing
          If (sLongFile.EndsWith("html") Or sLongFile.EndsWith("htm")) Then
            hd.Response_ContentType = "text/html"
          Else
            hd.Response_ContentType = "application/octet-stream"
          End If

          hd.Response_Body = FileIntoByteArray(sLongFile)
          hd.Response_ContentLength = hd.Response_Body.Length
          hd.Response_Number = HttpData.HttpResponseCodes.HTTP_OK
          hd.Response_Filename = sShortFile
          ' Get responses from Plugins
        Case Else  ' Other process or Invalid URL
          If ProcessWebRequest IsNot Nothing Then
            ProcessWebRequest(hd)

          Else '''''''''''FAILURE'''''''''''''''''''''''''''''''

            sBody += "Failure: Server does not support this URL<br /><br />" & vbCrLf
            If hd.Request_Headers.Contains("Host") Then
              sBody += "<font color=red>" & hd.Request_Headers("Host").trim & hd.Request_Filename & "</font><br /><br />"
              sBody += ByteArrayToString(hd.Request_Body)
            Else
              sBody += "<font color=red>" & hd.Request_Filename & "</font><br /><br />"
            End If

            sBody += "Server time : " & Now & "<br>" & vbCrLf
            hd.Response_Number = HttpData.HttpResponseCodes.HTTP_OK
            hd.Response_Text = "Invalid URL Found"
          End If

      End Select

      If sBody <> "" Then
        hd.Response_Body = ASCIIEncoding.ASCII.GetBytes(m_sHeader & sBody & m_sFooter)
      End If

      'Make sure this matches what the plugin creates.
      hd.Response_ContentLength = hd.Response_Body.Length

      '  Send the headers
      aucHeaders = hd.BuildHttpHeader()
      SendDataToClient(tc, aucHeaders, aucHeaders.Length)

      ' Send Content if required
      If (hd.Response_Number <> HttpData.HttpResponseCodes.HTTP_MOVED_PERMANENTLY) Then


        If hd.Response_ContentLength > 0 Then
          SendDataToClient(tc, hd.Response_Body, hd.Response_Body.Length)
        End If
      End If
    Catch ex As Exception
      'set an error message
      sBody += DateTime.Now.ToLongTimeString &
     " : HTTP Server Error Parsing URL: " & ex.Message
      RaiseEvent HttpError(sBody)
    End Try

  End Sub

  Public Sub SendDataToClient(ByRef tc As ThreadedClient,
                ByRef auc() As Byte,
                ByVal iLen As Integer)
    Dim ns As NetworkStream = tc.TcpClient.GetStream
    Dim iSendSize As Integer
    Dim iPos As Integer = 0
    Do
      iSendSize = Math.Min((iLen - iPos), tc.TcpClient.SendBufferSize)
      ns.Write(auc, iPos, iSendSize) ' blocking write
      iPos += iSendSize
    Loop While iPos < iLen
    ns.Flush()

  End Sub

End Class

'The ThreadedClient class encapsulates the functionality of a TcpClient connection with streaming for a single user.
Public Class ThreadedClient
  Public Connected As Boolean
  Public TcpClient As TcpClient
  Public ClientThread As Threading.Thread
  Public sExtraData As String
  Public sExtraValues As ValueType

  Public Event ClientConnect(ByVal tc As ThreadedClient) ' TODO Can these be ByRef?
  Public Event FirstPacketReceived(ByVal tc As ThreadedClient, ByVal baRecvBytes() As Byte)
  Public Event ClientClose(ByVal tc As ThreadedClient)
  Public Event ClientError(ByVal tc As ThreadedClient, ByVal sErr As String)

  Public Sub New(ByVal MyTCPClient As TcpClient)
    TcpClient = MyTCPClient
    'Connected = True ' TODO - should this line be moved into DoClientListen?
    ClientThread = New Thread(AddressOf DoClientListen)
    ClientThread.Priority = ThreadPriority.AboveNormal
    ClientThread.Name = "Client Thread " & Hour(Now) & Minute(Now) & Second(Now) & Rnd()
  End Sub

  Private Sub DoClientListen()
    'listen for packets from the client
    'use an asyc method here
    'Debug.WriteLine("Thread Creation: " & ClientThread.Name)
    Dim bLoop As Boolean = True
    Try
      Dim iBytesRead As Integer
      Dim baReadBuffer(TcpClient.ReceiveBufferSize) As Byte
      Dim networkStream As NetworkStream = TcpClient.GetStream()
      RaiseEvent ClientConnect(Me)
      Connected = True
      TcpClient.ReceiveTimeout = 1000 ' Client is connected - should send data...

      If (Connected = True) AndAlso (TcpClient.Client.Connected = True) Then ' TODO - Why both here?

        iBytesRead = networkStream.Read(baReadBuffer, 0, CInt(TcpClient.ReceiveBufferSize))
        If iBytesRead > 0 Then

          Dim baRecvBytes(0 To iBytesRead - 1) As Byte ' TODO - Why two buffers in this function?
          Array.Copy(baReadBuffer, baRecvBytes, iBytesRead)

          ' The entire process packets is started here.
          ' TODO - should this be passing ME, nothing or tcpClient?
          RaiseEvent FirstPacketReceived(Me, baRecvBytes)
        End If
      End If

    Catch ex As Exception
      RaiseEvent ClientError(Me, ex.Message)
    Finally
      If (Connected = True) OrElse
         ((TcpClient IsNot Nothing) AndAlso
          (TcpClient.Client IsNot Nothing) AndAlso
          (TcpClient.Client.Connected = True)) Then
        RaiseEvent ClientClose(Me)
      End If
    End Try
  End Sub

  Public Sub CloseClienHttpConn()
    On Error Resume Next
    Connected = False
    TcpClient.GetStream.Flush() '    Dim ns As NetworkStream = Client.GetStream  : ns.Flush()
    TcpClient.Close()
    RaiseEvent ClientClose(Me)
  End Sub

  Public Sub SendData(ByVal Data As String)
    If Connected = False Then Exit Sub
    Dim writer As New IO.StreamWriter(TcpClient.GetStream)
    writer.Write(Data)
    writer.Flush()
    writer.Close()
    writer.Dispose()
    writer = Nothing
  End Sub

End Class

Public Class HttpData

  ' old
  'Public Request_Valid As Boolean = False
  'Public Request_RemoteEndPoint As String = ""
  'Public Request_CompleteURL As String = ""

  'Public Request_Method As String = ""
  'Public Request_HTTPVersion As String = ""
  'Public Request_ConnectionType As String = ""
  'Public Request_Filename As String = ""
  'Public Request_LocalIsDir As Boolean = False

  ' Only supporting HTTP 1.1 Features!!

  Public Enum HttpResponseCodes
    'INFORMATIONAL_1XX
    HTTP_CONTINUE = 100
    HTTP_SWITCHING_PROTOCOLS = 101
    'SUCCESSFUL_2XX
    HTTP_OK = 200
    HTTP_CREATED = 201
    HTTP_ACCEPTED = 202
    HTTP_NON_AUTHORITATIVE_INFORMATION = 203
    HTTP_NO_CONTENT = 204
    HTTP_RESET_CONTENT = 205
    HTTP_PARTIAL_CONTENT = 206
    'REDIRECTION_3XX
    HTTP_MULTIPLE_CHOICES = 300
    HTTP_MOVED_PERMANENTLY = 301
    HTTP_FOUND = 302
    HTTP_SEE_OTHER = 303
    HTTP_NOT_MODIFIED = 304
    HTTP_USE_PROXY = 305
    HTTP_UNUSED = 306
    HTTP_TEMPORARY_REDIRECT = 307
    'CLIENT_ERROR_4XX
    HTTP_BAD_REQUEST = 400
    HTTP_UNAUTHORIZED = 401
    HTTP_PAYMENT_REQUIRED = 402
    HTTP_FORBIDDEN = 403
    HTTP_NOT_FOUND = 404
    HTTP_METHOD_NOT_ALLOWED = 405
    HTTP_NOT_ACCEPTABLE = 406
    HTTP_PROXY_AUTHENTICATION_REQUIRED = 407
    HTTP_CONFLICT = 409
    HTTP_GONE = 410
    HTTP_LENGTH_REQUIRED = 411
    HTTP_PRECONDITION_FAILED = 412
    HTTP_REQUEST_ENTITY_TOO_LARGE = 413
    HTTP_REQUEST_URI_TOO_LONG = 414
    HTTP_UNSUPPORTED_MEDIA_TYPE = 415
    HTTP_REQUESTED_RANGE_NOT_SATISFIABLE = 416
    HTTP_EXPECTATION_FAILED = 417
    'SERVER_ERROR_5XX
    HTTP_INTERNAL_SERVER_ERROR = 500
    HTTP_NOT_IMPLEMENTED = 501
    HTTP_BAD_GATEWAY = 502
    HTTP_SERVICE_UNAVAILABLE = 503
    HTTP_GATEWAY_TIMEOUT = 504
    HTTP_VERSION_NOT_SUPPORTED = 505
  End Enum

  Public Request_Head As String
  Public Request_Body As Byte()

  Public Request_Headers As Hashtable
  ' keys to this hash are going to look like this
  'Host: 
  'Connection: 
  'User-Agent: 
  'Cache-Control: 
  'Accept: 
  'Accept-Encoding: 
  'Accept-Language: 
  'Accept-Charset: 
  'Location: 
  ' These will be interpreted from the first line of the http head
  ' GET / HTTP/1.1  or POST /my_post_route/ HTTP/1.1
  Public Request_Method As String
  Public Request_Filename As String
  Public Request_HTTPVersion As String
  Public Request_Query As String
  Public Request_QueryParams As Hashtable

  ' This has to be pulled out of the TCPIP Client information
  Public Request_RemoteIP As String
  Public Request_RemotePort As Integer

  ' This is a unique way to identify this request
  Public Request_ID As String

  Public Request_ContentLength As Integer
  Public Request_ContentLengthReceived As Integer ' tells us where we are in the pointer
  Public Request_ContentComplete As Boolean

  ''''RESPONSES'''''''''''''''''''''''
  Public Response_Head As String
  Public Response_Body As Byte()

  ' first line of HTTP Response looks like:
  '    Request_HTTPVersion & " " & Response_Number & " " & Response_Text 
  Public Response_Number As HttpResponseCodes
  Public Response_Text As String
  Public Response_ContentLength As Integer
  Public Response_ContentType As String
  Public Response_ServerType As String
  Public Response_Version As String
  Public Response_Date As String
  Public Response_Connection As String
  Public Response_Filename As String
  Public Response_Redirect_Location As String

  ' Builds the Response_Headers string and returns it as a byte array
  Public Function BuildHttpHeader() As Byte()
    Dim sHeader As String
    If Response_Redirect_Location IsNot Nothing AndAlso Response_Redirect_Location <> "" Then
      Response_Number = HttpResponseCodes.HTTP_MOVED_PERMANENTLY
      Response_Text = "Moved Permanently"
    End If

    sHeader = Request_HTTPVersion & " " & Response_Number & " " & Response_Text & vbCrLf
    sHeader += "Date: " & Response_Date & vbCrLf
    sHeader += "Server: " & Response_ServerType & "/" & GetExecutingAssembly().GetName().Version.ToString() & vbCrLf
    '    sHeader += "Last-Modified: " & Response_Date & vbCrLf
    sHeader += "Content-Type: " & Response_ContentType & vbCrLf

    If Response_Redirect_Location IsNot Nothing AndAlso Response_Redirect_Location <> "" Then
      sHeader += "Location: " & Response_Redirect_Location & vbCrLf
    Else

      'If we don't have any content to send after this, ditch it
      If Response_ContentLength > 0 Then
        sHeader += "Accept-Ranges: bytes" & vbCrLf
        sHeader += "Content-Length: " & Response_ContentLength & vbCrLf
        ' done later sHeader += "Content-Disposition: filename=" & Response_Filename & vbCrLf
        ' The below line will indicate a "File Download" dialog box for a known MIME type
        'sHeader += "Content-Disposition: attachment; filename=" &  Response_Filename & vbCrLf
      End If

      ' correctly fill the Content-Disposition if needed
      ': attachment; filename=<file name.ext> 
      Dim sContentDisposition As String, sExtension As String
      sExtension = GetContentDispositionExtension(Response_ContentType)
      If Response_ContentType.Contains("text") Or Response_ContentType.Contains("image") Then
        sContentDisposition = "Content-Disposition: filename=<file name>"
      Else
        sContentDisposition = "Content-Disposition: attachment; filename=<file name>"
      End If

      If (Response_Filename Is Nothing) OrElse (Response_Filename.Trim.Length = 0) Then ' if filename is null, give it one
        Response_Filename = Request_ID
      End If

      If Response_Filename.IndexOf(".") < 0 Then
        Response_Filename += "." & sExtension
      End If
      sHeader += sContentDisposition.Replace("<file name>", Response_Filename).Trim & vbCrLf
    End If
    ' TODO add Content-Transfer-Encoding: header

    ' End the headers here !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    sHeader += vbCrLf

    Response_Head = sHeader

    Return Encoding.ASCII.GetBytes(Response_Head)
  End Function

  Private Function GetContentDispositionExtension(ByVal sContentDisposition As String) As String
    Dim sExt As String = ""
    Select Case sContentDisposition
      Case "application/EDI-X12" ' EDI X12 data; Defined in RFC 1767
        sExt = "EDI-X12"
      Case "application/EDIFACT" ' EDI EDIFACT data; Defined in RFC 1767
        sExt = "EDIFACT"
      Case "application/json" ' JavaScript Object Notation JSON; Defined in RFC 4627
        sExt = "json"
      Case "application/javascript" ' JavaScript; Defined in RFC 4329 but not accepted in IE 8 or earlier
        sExt = "js"
      Case "application/octet-stream" ' Arbitrary binary data[4].
        sExt = "bin"
      Case "application/ogg" ' Ogg, a multimedia bitstream container format; Defined in RFC 5334
        sExt = "ogg"
      Case "application/pdf" ' Portable Document Format, PDF has been in use for document exchange on the Internet since 1993; Defined in RFC 3778
        sExt = "pdf"
      Case "application/soap+xml" ' SOAP; Defined by RFC 3902
        sExt = "soap"
      Case "application/xhtml+xml" ' XHTML; Defined by RFC 3236
        sExt = "xhtml"
      Case "application/xml-dtd" ' DTD files; Defined by RFC 3023
        sExt = "xml"
      Case "application/zip" ' ZIP archive files; Registered[6]
        sExt = "zip"
      Case "audio/basic" ' mulaw audio at 8 kHz, 1 channel; Defined in RFC 2046
        sExt = "mulaw"
      Case "audio/mp4" ' MP4 audio
        sExt = "mp4"
      Case "audio/mpeg" ' MP3 or other MPEG audio; Defined in RFC 3003
        sExt = "mpeg"
      Case "audio/ogg" ' Ogg Vorbis, Speex, Flac and other audio; Defined in RFC 5334
        sExt = "ogg"
      Case "audio/vorbis" ' Vorbis encoded audio; Defined in RFC 5215
        sExt = "vorbis"
      Case "audio/x-ms-wma" ' Windows Media Audio; Documented in Microsoft KB 288102
        sExt = "wma"
      Case "audio/x-ms-wax" ' Windows Media Audio Redirector; Documented in Microsoft help page
        sExt = "wax"
      Case "audio/vnd.rn-realaudio" ' RealAudio; Documented in RealPlayer Customer Support Answer 2559
        sExt = "rn"
      Case "audio/vnd.wave" ' WAV audio; Defined in RFC 2361
        sExt = "wav"
      Case "image/gif" ' GIF image; Defined in RFC 2045 and RFC 2046
        sExt = "gid"
      Case "image/jpeg" ' JPEG JFIF image; Defined in RFC 2045 and RFC 2046
        sExt = "jpg"
      Case "image/png" ' Portable Network Graphics; Registered[7], Defined in RFC 2083
        sExt = "png"
      Case "image/svg+xml" ' SVG vector image; Defined in SVG Tiny 1.2 Specification Appendix M
        sExt = "svg"
      Case "image/tiff" ' Tag Image File Format; Defined in RFC 3302
        sExt = "tiff"
      Case "image/x-icon", "image/vnd.microsoft.icon" ' ICO image; Registered[8]
        sExt = "ico"
      Case "message/http"
        sExt = "html"
      Case "multipart/mixed" ' MIME E-mail; Defined in RFC 2045 and RFC 2046
        sExt = "mime"
      Case "multipart/alternative" ' MIME E-mail; Defined in RFC 2045 and RFC 2046
        sExt = "mime"
      Case "multipart/related" ' MIME E-mail; Defined in RFC 2387 and used by MHTML (HTML mail)
        sExt = "mime"
      Case "multipart/form-data" ' MIME Webform; Defined in RFC 2388
        sExt = "mime"
      Case "multipart/signed" ' Defined in RFC 1847
        sExt = "mime"
      Case "multipart/encrypted" ' Defined in RFC 1847
        sExt = "mime"
      Case "text/css" ' Cascading Style Sheets; Defined in RFC 2318
        sExt = "css"
      Case "text/csv" ' Comma-separated values; Defined in RFC 4180
        sExt = "csv"
      Case "text/html" ' HTML; Defined in RFC 2854
        sExt = "html"
      Case "text/plain" ' Textual data; Defined in RFC 2046 and RFC 3676
        sExt = "txt"
      Case "text/xml" ' Extensible Markup Language; Defined in RFC 3023
        sExt = "xml"
      Case "video/mpeg" ' MPEG-1 video with multiplexed audio; Defined in RFC 2045 and RFC 2046
        sExt = "mpeg"
      Case "video/mp4" ' MP4 video; Defined in RFC 4337
        sExt = "mp4"
      Case "video/ogg" ' Ogg Theora or other video (with audio); Defined in RFC 5334
        sExt = "ogg"
      Case "video/quicktime" ' QuickTime video; Registered[9]
        sExt = "qt"
      Case "video/webm" ' WebM open media format
        sExt = "webm"
      Case "video/x-ms-wmv" ' Windows Media Video; Documented in Microsoft KB 288102
        sExt = "wmv"
      Case "application/vnd.oasis.opendocument.text" ' OpenDocument Text; Registered [10]
        sExt = "odt"
      Case "application/vnd.oasis.opendocument.spreadsheet" ' OpenDocument Spreadsheet; Registered [11]
        sExt = "ods"
      Case "application/vnd.oasis.opendocument.presentation" ' OpenDocument Presentation; Registered [12]
        sExt = "odp"
      Case "application/vnd.oasis.opendocument.graphics" ' OpenDocument Graphics; Registered [13]
        sExt = "odg"
      Case "application/vnd.ms-excel" ' Microsoft Excel files
        sExt = "xls"
      Case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" ' Microsoft Excel 2007 files
        sExt = "ods"
      Case "application/vnd.ms-powerpoint" ' Microsoft Powerpoint files
        sExt = "ppt"
      Case "application/vnd.openxmlformats-officedocument.presentationml.presentation" ' Microsoft Powerpoint 2007 files
        sExt = "odp"
      Case "application/msword" ' Microsoft Word files
        sExt = "doc"
      Case "application/vnd.openxmlformats-officedocument.wordprocessingml.document" ' Microsoft Word 2007 files
        sExt = "odt"
      Case "application/vnd.mozilla.xul+xml" ' Mozilla XUL files
        sExt = "xul"
      Case "application/x-www-form-urlencoded Form Encoded Data; Documented in HTML 4.01 Specification, Section 17.13.4.1"
        sExt = "form-urlencoded"
      Case "application/x-dvi" ' device-independent document in DVI format
        sExt = "dvi"
      Case "application/x-latex" ' LaTeX files
        sExt = "latex"
      Case "application/x-font-ttf" ' TrueType Font No registered MIME type, but this is the most commonly used
        sExt = "ttf"
      Case "application/x-shockwave-flash" ' Adobe Flash files for example with the extension .swf; Documented in Adobe TechNote tn_4151 and Adobe TechNote tn_16509
        sExt = "flash"
      Case "application/x-stuffit" ' StuffIt archive files
        sExt = "stuffit"
      Case "application/x-rar-compressed" ' RAR archive files
        sExt = "rar"
      Case "application/x-tar" ' Tarball files
        sExt = "tar"
      Case "application/x-pkcs12" ' p12 files
        sExt = "p12"
      Case "application/x-pkcs12" ' pfx files
        sExt = "pfx"
      Case "application/x-pkcs7-certificates" ' p7b files
        sExt = "p7b"
      Case "application/x-pkcs7-certificates" ' spc files
        sExt = "spc"
      Case "application/x-pkcs7-certreqresp" ' p7r files
        sExt = "p7r"
      Case "application/x-pkcs7-mime" ' p7c files
        sExt = "p7c"
      Case "application/x-pkcs7-mime" ' p7m files
        sExt = "p7m"
      Case "application/x-pkcs7-signature" ' p7s files
        sExt = "p7s"
      Case Else
        sExt = "unknown"
    End Select
    Return sExt
  End Function

  Public Sub ParseHttpRequest(ByRef ba() As Byte,
    ByVal bInitialPacket As Boolean)

    'GET /test/a%20picture.jpg HTTP/1.1
    'Accept: image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/vnd.ms-powerpoint, application/vnd.ms-excel, application/msword, application/x-shockwave-flash, */*
    'Accept-Language: en-us
    'Accept-Encoding: gzip, deflate
    'User-Agent: Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.0.3705; .NET CLR 1.1.4322)
    'Host: 127.0.0.1:81
    'Connection: Keep-Alive
    Dim iPos As Integer, iLenHeader As Integer, iSizeBodyDataRecvInThisPacket As Integer
    Dim asSplit() As String
    Dim sFirstLine As String, sKey As String, sVal As String, sData As String

    Try
      If bInitialPacket Then
        sData = ByteArrayToString(ba)

        Response_Number = HttpResponseCodes.HTTP_PARTIAL_CONTENT
        Response_Text = "OK"
        Response_ContentLength = 0
        Response_ContentType = "text/html; charset=utf-8"
        Response_ServerType = "XPressSync Server"
        Response_Version = "HTTP/1.1"
        Response_Date = String.Format("{0:R}", Now)
        Response_Connection = "close"
        Request_ContentComplete = False
        Request_ContentLength = 0
        Request_Headers = New Hashtable

        ' Read the fist line of the Request_Head and interpret it
        iPos = InStr(sData, vbCrLf)
        sFirstLine = sData.Substring(0, iPos).Trim

        asSplit = Split(sFirstLine)
        If (asSplit.Length <> 3) Then
          Response_Text = "Badly formed HTTP header" & vbCrLf
          Response_Number = HttpResponseCodes.HTTP_BAD_REQUEST
          Return
        End If

        ' Pull out the type, filename, and http version
        Request_Method = asSplit(0).Trim.ToUpper
        Request_Filename = asSplit(1).Trim
        Request_HTTPVersion = asSplit(2).Trim

        If Not ((Request_Method = "GET") Or (Request_Method = "POST")) Then
          Response_Text = """GET"" or ""POST"" Header strings not found""" & vbCrLf
          Response_Number = HttpResponseCodes.HTTP_BAD_REQUEST
          Return
        End If

        'Get the URL Query Parameters
        iPos = Request_Filename.IndexOf("?")
        If iPos < 0 Then
          Request_Query = ""
        Else
          Request_Query = Request_Filename.Substring(iPos + 1)
        End If
        Request_QueryParams = HashTableFromString(Request_Query)

        ' Create unique identifier for connection 
        Request_ID = Now.ToString("yyyyMMddHHmmssfff_") & (New Random).Next

        ' this will be fixed upon successful completion of the service.
        iLenHeader = sData.IndexOf(vbCrLf & vbCrLf)           ' Find the 2xcrlf indicating end of the header
        If iLenHeader = -1 Then iLenHeader = sData.Length ' if it isn't there, put all data into header
        Request_Head = sData.Substring(0, iLenHeader) ' copy this into request header

        ' parse header data into hash table 
        asSplit = Split(Request_Head.Replace(sFirstLine & vbCrLf, ""), vbCrLf)  ' remove GET or POST line
        For Each sHeaderLine As String In asSplit
          iPos = sHeaderLine.IndexOf(":")
          If iPos > 1 Then
            sKey = sHeaderLine.Substring(0, iPos).Trim
            sVal = sHeaderLine.Substring(iPos + 1).Trim
            Request_Headers.Add(sKey, sVal)
          End If
        Next

        ' See if we have a "Content-Length" entry - if so, we have a body
        If Request_Headers.ContainsKey("Content-Length") Then
          Request_ContentLength = CInt(Request_Headers("Content-Length"))
          ReDim Request_Body(0 To Request_ContentLength - 1) ' make the body length the appropriate size

          iSizeBodyDataRecvInThisPacket = ba.Length - (Request_Head.Length + 4) ' Separate out the head and data
          Array.Copy(ba, Request_Head.Length + 4, Request_Body, 0, iSizeBodyDataRecvInThisPacket)
          Request_ContentLengthReceived = iSizeBodyDataRecvInThisPacket

          If Request_ContentLengthReceived = Request_ContentLength Then ' then we have the entire packet
            Request_ContentComplete = True
          Else
            Request_ContentComplete = False
          End If
        Else ' no content --> we have everything!
          Request_ContentComplete = True
          Request_ContentLengthReceived = 0
          Request_ContentLength = 0
          ReDim Request_Body(-1)
        End If

      Else ' We have extra GET or POST data in a separate packet

        iSizeBodyDataRecvInThisPacket = ba.Length
        Array.Copy(ba, 0, Request_Body, Request_ContentLengthReceived, iSizeBodyDataRecvInThisPacket)
        Request_ContentLengthReceived += iSizeBodyDataRecvInThisPacket

        If Request_ContentLengthReceived = Request_ContentLength Then ' then we have the entire packet
          Request_ContentComplete = True
        Else
          Request_ContentComplete = False
        End If

      End If

      ' Fill in good response information
      If Request_ContentComplete Then
        Response_Number = HttpResponseCodes.HTTP_OK
      End If
    Catch ex As Exception
      Response_Number = HttpResponseCodes.HTTP_BAD_REQUEST
      Debug.Print(ex.Message)

    End Try

  End Sub

  ' TODO: Removed need for this - might be useful function though...

  'Public Function AppendContent(ByRef aucAppendTo() As Byte,
  '                         ByRef aucNew() As Byte,
  '                         ByVal iSizeBuf As Integer,
  '                         Optional ByVal bReduceInputBuffer As Boolean = True) As Boolean
  '  Dim iBytesLeftOver As Integer = aucNew.Length - iSizeBuf
  '  If (iSizeBuf <= 0) Or (iBytesLeftOver < 0) Then Return False

  '  Dim iAppendedLen As Integer = iSizeBuf
  '  ' Add the old size (if it has one) to get the new content length
  '  If Request_Body IsNot Nothing Then
  '    iAppendedLen += Request_Body.Length
  '  End If

  '  ReDim Preserve aucAppendTo(0 To iAppendedLen - 1)  ' Create buffer for return value
  '  aucAppendTo.CopyTo(aucNew, Request_Body.Length)    ' Copy from the input array

  '  ' Reduce the input array appropriately 
  '  If bReduceInputBuffer AndAlso (iBytesLeftOver > 0) Then
  '    Array.Copy(aucNew, iSizeBuf, aucNew, 0, iBytesLeftOver)
  '    ReDim Preserve aucNew(0 To iBytesLeftOver - 1)
  '  End If
  ' Return True

  'End Function


  Public Function Request_BodyAscii() As String
    Return ByteArrayToString(Request_Body)
  End Function

  Public Function Response_BodyAscii() As String
    Return ByteArrayToString(Response_Body)
  End Function


  Private Shared Function ByteArrayToString(ByRef ba As Byte()) As String
    Dim sContent As String = ""
    If ba IsNot Nothing Then
      sContent = Encoding.ASCII.GetString(ba, 0, ba.Length)
    End If
    Return sContent
  End Function

  Public Shared Function URLDecode(ByVal StringToDecode As String) As String
    ' This is needed for parsing out "POST" parameters

    Dim TempAns As String = ""
    Dim CurChr As Integer

    CurChr = 1

    Do Until CurChr - 1 = Len(StringToDecode)
      Select Case Mid(StringToDecode, CurChr, 1)
        Case "+"
          TempAns = TempAns & " "
        Case "%"
          TempAns = TempAns & Chr(Val("&h" & _
           Mid(StringToDecode, CurChr + 1, 2)))
          CurChr = CurChr + 2
        Case Else
          TempAns = TempAns & Mid(StringToDecode, CurChr, 1)
      End Select

      CurChr = CurChr + 1
    Loop

    URLDecode = TempAns
  End Function

  '===============================================================================
  '    Name: Sub HashTableFromString
  ' Remarks:
  '===============================================================================
  Public Shared Function HashTableFromString(ByVal sURL As String, _
    Optional ByVal sSectionSplit As String = "&", _
    Optional ByVal sKeyValueSplit As String = "=", _
    Optional ByVal sIgnoreBefore As String = "?", _
    Optional ByVal sIgnoreBeforeKeyVal As String = " ") As Hashtable
    Dim ht As New Hashtable
    Dim asSections() As String
    Dim asKeyVal() As String
    Dim iPos As Integer = sURL.IndexOf(sIgnoreBefore)
    Dim trim_char() As Char = {"&"}
    asSections = Split(sURL.Substring(iPos + sIgnoreBefore.Length), sSectionSplit)

    For Each sSection As String In asSections
      asKeyVal = Split(sSection, sKeyValueSplit)
      If asKeyVal.Length = 2 Then
        ht.Add(asKeyVal(0).TrimStart(trim_char).Trim, asKeyVal(1).Trim)
      End If
    Next

    Return ht
  End Function
End Class

Public Class cWriteLog
  Public [Data] As String = ""
  Public [Function] As String
  Public Sub New(ByVal sData As String, ByVal sFunction As String)
    [Data] = sData
    [Function] = sFunction
  End Sub

End Class

