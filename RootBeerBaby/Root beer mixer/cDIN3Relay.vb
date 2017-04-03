Imports System
Imports System.Net
Imports System.ComponentModel

<DefaultEvent("CommandResult")> <System.ComponentModel.DesignerCategory("")> _
Public Class DIN3Relay

#Region "Initialization"

  'declaration of variables to be used 
  Private m_bRelayStatus() As Boolean     'returns true or false whether or not relay is on or off
  Private m_strRelayName() As String      'gives the number of the relay that you are referring to on the unit
  Private m_strAddress As String = ""     'the string for the ipaddress of the relay unit
  Private ClearProxy As Boolean           '"Just google it, find out what it is" - Alex
  Private TimeoutMs As Integer            'specifying the timeout for the webrequest

  ''' <summary>
  ''' Use this sub to create a new object out of your relay unit in code. Each object will have its own specific set of parameters.
  ''' </summary>
  ''' <param name="strIpAddress"></param>
  ''' <param name="strLogin"></param>
  ''' <param name="strPassword"></param>
  ''' <param name="bClearProxy"></param>
  ''' <param name="iTimeoutMs"></param>
  ''' <remarks></remarks>
  Public Sub New(strIpAddress As String, strLogin As String, strPassword As String, Optional bClearProxy As Boolean = True, Optional iTimeoutMs As Integer = 5000)
    If strIpAddress.Trim = "" Then
      Throw New Exception("Missing IP Address")
    End If
    If strLogin.Trim = "" Then
      Throw New Exception("Missing Login")
    End If
    m_bRelayStatus = New Boolean(0 To 8) {}
    m_strRelayName = New String(0 To 8) {}
    IpAddress = strIpAddress
    Login = strLogin
    Password = strPassword
    ClearProxy = bClearProxy
    TimeoutMs = iTimeoutMs
    SetAllowUnsafeHeaderParsing20()
  End Sub

#End Region

#Region "Properties"

  'This section is where you define your properties for your relay unit.
  'These properties are used when you are setting up a new instance of a relay in your code.

  Private m_strIpAddress As String
  <System.ComponentModel.Category("Device")> _
  <System.ComponentModel.DisplayName("IP Address")> _
  <System.ComponentModel.Description("IP Address of The DIN Relay III device")> _
  Public Property IpAddress() As String
    Get
      Return m_strIpAddress
    End Get
    Set(value As String)
      m_strIpAddress = value
    End Set
  End Property

  Private m_strLogin As String
  <System.ComponentModel.Category("User")> _
  <System.ComponentModel.Description("User login")> _
  Public Property Login() As String
    Get
      Return m_strLogin
    End Get
    Set(value As String)
      m_strLogin = value
    End Set
  End Property

  Private m_strPassword As String
  <System.ComponentModel.Category("User")> _
  <System.ComponentModel.Description("User password")> _
  <System.ComponentModel.PasswordPropertyText(True)> _
  Public Property Password() As String
    Get
      Return m_strPassword
    End Get
    Set(value As String)
      m_strPassword = value
    End Set
  End Property

  Private m_WebRequest As WebClientWithTimeout
  Private Property WebRequest() As WebClientWithTimeout
    Get
      Return m_WebRequest
    End Get
    Set(value As WebClientWithTimeout)
      If m_WebRequest IsNot value Then
        If m_WebRequest IsNot Nothing Then
          RemoveHandler m_WebRequest.DownloadStringCompleted, New DownloadStringCompletedEventHandler(AddressOf WebRequest_DownloadStringCompleted)
        End If
        m_WebRequest = value
        If m_WebRequest IsNot Nothing Then
          AddHandler m_WebRequest.DownloadStringCompleted, New DownloadStringCompletedEventHandler(AddressOf WebRequest_DownloadStringCompleted)
        End If
      End If
    End Set
  End Property

  Private ReadOnly Property DeviceUri() As String
    Get
      Dim strRet As String
      strRet = String.Format("http://{0}/", IpAddress)
      Return strRet
    End Get
  End Property

  Private m_ControllerName As String
  Public Property ControllerName() As String
    Get
      Return m_ControllerName
    End Get
    Set(value As String)
      m_ControllerName = value
    End Set
  End Property

#End Region

#Region "Events"

  'This area is the area where events for the events for the relay unit are specified.
  'I suppose that actually isn't helpful, considering that is what the region is called...

  Public Event StatusChanged As EventHandler

  <Category("Custom Events"), DescriptionAttribute("The result of the connection attempt.")> _
  Public Event CommandResult(ByRef bResult As Boolean, ByRef strResult As String, ByRef strSourceAddress As String)

  <Category("Custom Events"), DescriptionAttribute("Indicates a command was sent.")> _
  Public Event CommandSent(ByRef strSourceAddress As String)

#End Region

#Region "Connect and Base WebRequest Functions"

  'This region is really import to the functioning of the relay units.
  'All of these functions have been named so that it is possible to get a basic idea of what they are doing from the start.

  Public Function IsConnected() As Boolean
    Return My.Computer.Network.Ping(My.Settings.IPAddress)
  End Function

  Public Function IsConnected2() As Boolean
    Return My.Computer.Network.Ping(My.Settings.IPAddress2)
  End Function

  Public Function IsConnected3() As Boolean
    Return My.Computer.Network.Ping(My.Settings.IPAddress3)
  End Function

  Public Function Connect() As Boolean
    m_ControllerName = "Not Connected"
    m_bRelayStatus = New Boolean(0 To 8) {}
    m_strRelayName = New String(0 To 8) {}
    WebRequest = CreateWebRequest()
    Return True
  End Function

  Private Function CreateWebRequest() As WebClientWithTimeout
    Debug.Print(Now.ToLocalTime & " : WebRequest Started : Timeout " & TimeoutMs)
    Dim objRet As WebClientWithTimeout = New WebClientWithTimeout(TimeoutMs)
    If ClearProxy = True Then objRet.Proxy = Nothing
    Dim strLoginPassword As String = String.Format("{0}:{1}", Login, Password)
    Dim bytLoginPassword As Byte() = System.Text.Encoding.UTF8.GetBytes(strLoginPassword)
    Dim strLoginPasswordEncoded As String = Convert.ToBase64String(bytLoginPassword)
    objRet.Headers.Add(String.Format("Authorization: Basic {0}", strLoginPasswordEncoded))
    Return objRet
  End Function

  Private Sub WebRequest_DownloadStringCompleted(sender As Object, e As DownloadStringCompletedEventArgs)
    If Not e.Cancelled Then
      If e.[Error] Is Nothing Then
        If e.UserState IsNot Nothing Then
          If e.UserState.ToString() = "RefreshStatus" Then
            RefreshStatus_Response(e.Result)
          End If
        End If
        RaiseEvent CommandResult(True, "Command Completed Successfully", m_strAddress)
      Else
        RaiseEvent CommandResult(True, "Error: " & DirectCast(e.Error, System.Net.WebException).Status.ToString, m_strAddress)
      End If
      m_strAddress = ""
    End If
  End Sub

  Public Shared Function SetAllowUnsafeHeaderParsing20() As Boolean
    'Get the assembly that contains the internal class
    Dim aNetAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(System.Net.Configuration.SettingsSection))
    If aNetAssembly IsNot Nothing Then
      'Use the assembly in order to get the internal type for the internal class
      Dim aSettingsType As Type = aNetAssembly.[GetType]("System.Net.Configuration.SettingsSectionInternal")
      If aSettingsType IsNot Nothing Then
        'Use the internal static property to get an instance of the internal settings class.
        'If the static instance isn't created allready the property will create it for us.
        Dim anInstance As Object = aSettingsType.InvokeMember("Section", System.Reflection.BindingFlags.[Static] Or System.Reflection.BindingFlags.GetProperty Or System.Reflection.BindingFlags.NonPublic, Nothing, Nothing, New Object() {})
        If anInstance IsNot Nothing Then
          'Locate the private bool field that tells the framework is unsafe header parsing should be allowed or not
          Dim aUseUnsafeHeaderParsing As System.Reflection.FieldInfo = aSettingsType.GetField("useUnsafeHeaderParsing", System.Reflection.BindingFlags.NonPublic Or System.Reflection.BindingFlags.Instance)
          If aUseUnsafeHeaderParsing IsNot Nothing Then
            aUseUnsafeHeaderParsing.SetValue(anInstance, True)
            Return True
          End If
        End If
      End If
    End If
    Return False
  End Function

#End Region

#Region "Commands"
  ''' <summary>
  ''' You won't use this function much for our purposes. This uses the built in scripting functionality of the relay which we don't utilize in this application.
  ''' </summary>
  ''' <param name="intScriptLine"></param>
  ''' <remarks></remarks>
  Public Sub StartScript(intScriptLine As Integer)
    Dim strUri As String = String.Format("{0}script?run{1:000}", DeviceUri, intScriptLine)
    m_strAddress = strUri
    RaiseEvent CommandSent(m_strAddress)
    WebRequest.DownloadStringAsync(New Uri(strUri), Nothing)
  End Sub

  'For all of the functions listed below, you are using these to set individual relay states to on or off.
  'Some of the functions have been modified to do the same things just using different parameters.

  Public Sub Relay_Set(intRelay As Integer, blnState As Boolean)
    Dim strOnOff As String = BoolToText(blnState).ToUpper()
    Dim strAddress As String = String.Format("{0}outlet?{1}={2}", DeviceUri, intRelay, strOnOff)
    m_strAddress = strAddress
    RaiseEvent CommandSent(m_strAddress)
    WebRequest.DownloadStringAsync(New Uri(strAddress), "Relay Set")
    m_bRelayStatus(intRelay) = blnState
  End Sub

  Public Sub Relay_Set_ON(intRelay As Integer, blnState As Boolean)
    Dim strON As String = "ON"
    Dim strAddress As String = String.Format("{0}outlet?{1}={2}", DeviceUri, intRelay, strON)
    m_strAddress = strAddress
    RaiseEvent CommandSent(m_strAddress)
    WebRequest.DownloadString(New Uri(strAddress))
  End Sub

  Public Sub Relay_Set_OFF(intRelay As Integer, blnState As Boolean)
    Dim strOFF As String = "OFF"
    Dim strAddress As String = String.Format("{0}outlet?{1}={2}", DeviceUri, intRelay, strOFF)
    m_strAddress = strAddress
    RaiseEvent CommandSent(m_strAddress)
    WebRequest.DownloadString(New Uri(strAddress))
  End Sub

  Public Sub Relay_Toggle(intRelay As Integer)
    Relay_Set(intRelay, Not m_bRelayStatus(intRelay))
  End Sub

  Public Sub Relay_Toggle_ON(intRelay As Integer)
    Relay_Set_ON(intRelay, Not m_bRelayStatus(intRelay))
  End Sub

  Public Sub Relay_Toggle_OFF(intRelay As Integer)
    Relay_Set_OFF(intRelay, Not m_bRelayStatus(intRelay))
  End Sub

  Public Sub Relay_Set_ID(sRequest As String, strOnOff As String)
    Dim strAddress As String = String.Format(sRequest, DeviceUri, strOnOff)
    m_strAddress = strAddress
    RaiseEvent CommandSent(m_strAddress)
    WebRequest.DownloadStringAsync(New Uri(strAddress), "Relay Set ID")
  End Sub

  Public Sub Relay_SetAll(blnState As Boolean)
    Dim strOnOff As String = BoolToText(blnState).ToUpper()
    Dim strAddress As String = String.Format("{0}outlet?a={1}", DeviceUri, strOnOff)
    m_strAddress = strAddress
    RaiseEvent CommandSent(m_strAddress)
    WebRequest.DownloadStringAsync(New Uri(strAddress), "Relay_SetAll")
    For i As Integer = 1 To 8
      m_bRelayStatus(i) = blnState
    Next
  End Sub

  Public Sub Relay_SetAll_ON(blnstate As Boolean)
    Dim strOnOff As String = "ON"
    Dim strAddress As String = String.Format("{0}outlet?1={1}&2={1}&3={1}&4={1}&5={1}&6={1}&7={1}&8={1}", DeviceUri, strOnOff)
    m_strAddress = strAddress
    RaiseEvent CommandSent(m_strAddress)
    WebRequest.DownloadStringAsync(New Uri(strAddress), "All Relays ON")
    For i As Integer = 1 To 8
      m_bRelayStatus(i) = blnstate
    Next
  End Sub

  Public Sub RefreshStatus()
    Dim strUriIndex As String = String.Format("{0}index.htm", DeviceUri)
    m_strAddress = strUriIndex
    RaiseEvent CommandSent(m_strAddress)
    WebRequest.DownloadStringAsync(New Uri(strUriIndex), "Refresh Status")
  End Sub

#End Region

#Region "Strings for GUI"

  'This section of the code is not used much by us. This was used for old testing.

  Public Function Status(intRelay As Integer) As Boolean
    Return m_bRelayStatus(intRelay)
  End Function

  Public Function BoolToText(blnStatus As Boolean) As String
    Dim strRet As String
    If blnStatus Then
      strRet = "On"
    Else
      strRet = "Off"
    End If
    Return strRet
  End Function

  Private Function TextToBool(strState As String) As Boolean
    Dim blnRet As Boolean = False
    If strState.ToUpper() = "ON" Then
      blnRet = True
    End If
    Return blnRet
  End Function

  Public Function RelayName(i As Integer) As String
    Return m_strRelayName(i)
  End Function

#End Region

#Region "Parse DIN Relay III responses from HTML"

  'These are also not really used. This is used more for interacting with the relay over webrequests.

  Private Sub RefreshStatus_Response(strResponse As String)
    ControllerName = ParseControllerName(strResponse)
    For i As Integer = 1 To 8
      m_bRelayStatus(i) = ParseRelayState(strResponse, i)
      m_strRelayName(i) = ParseRelayName(strResponse, i)
    Next
  End Sub

  Private Function ParseControllerName(strResponse As String) As String
    Dim strRet As String = "Not Connected"
    Dim strFind As String = "Controller: "
    Dim intStart As Integer = strResponse.IndexOf(strFind)
    If intStart > 0 Then
      intStart = intStart + strFind.Length
      Dim intstop As Integer = strResponse.IndexOf(vbLf, intStart)
      If intstop > 0 Then
        strRet = strResponse.Substring(intStart, intstop - intStart)
        strRet = strRet.Trim()
      End If
    End If

    Return strRet
  End Function

  Private Function ParseRelayState(strResponse As String, intRelay As Integer) As Boolean
    Dim blnRet As Boolean = False
    Dim strFind As String = String.Format("href=outlet?{0}=0", intRelay)
    Dim intStart As Integer = strResponse.IndexOf(strFind)
    If intStart > 0 Then
      Dim strValue As String = strResponse.Substring(intStart + strFind.Length - 1, 2)
      blnRet = TextToBool(strValue)
      blnRet = Not blnRet
    End If
    Return blnRet
  End Function

  Private Function ParseRelayName(strResponse As String, intRelay As Integer) As String
    Dim strRet As String = String.Format("Switch {0}", intRelay)
    Dim strFind As String = String.Format("<td align=center>{0}</td>", intRelay)
    Dim intStart As Integer = strResponse.IndexOf(strFind)
    If intStart > 0 Then
      intStart += strFind.Length
      strFind = "<td>"
      intStart = strResponse.IndexOf(strFind, intStart)
      intStart += strFind.Length
      Dim intstop As Integer = strResponse.IndexOf("</td>", intStart)
      If intstop > 0 Then
        strRet = strResponse.Substring(intStart, intstop - intStart)
      End If
    End If
    Return strRet
  End Function

#End Region

  Protected Overrides Sub Finalize()
    MyBase.Finalize()
  End Sub

End Class

