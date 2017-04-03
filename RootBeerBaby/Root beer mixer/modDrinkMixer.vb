Public Module modDrinkMixer
  Public g_iPort As Integer = 10000
  Public g_httpServer As HTTPServer = Nothing

  Public Sub StartServerAndOthers()
    'Call to start webservice from both Service and Local App
    If g_httpServer Is Nothing Then Call StartTCPIPServer()
  End Sub

  Public Sub StartTCPIPServer()
    'Don't bother running if we're supposed to run from a service
    If (g_httpServer IsNot Nothing) AndAlso (g_httpServer.LocalPort <> g_iPort) Then
      Call StopTCPIPServer()
      g_httpServer = New HTTPServer(g_iPort, AddressOf ProcessWebRequest)
      g_httpServer.StartServer()
    ElseIf g_httpServer Is Nothing Then
      g_httpServer = New HTTPServer(g_iPort, AddressOf ProcessWebRequest)
      g_httpServer.StartServer()
    Else
      'The server already exists and the port hasn't changed, do nothing
    End If

  End Sub

  Public Sub StopTCPIPServer()
    If g_HTTPServer Is Nothing Then
    Else
      g_HTTPServer.StopServer()
      g_HTTPServer = Nothing
    End If
  End Sub





End Module
