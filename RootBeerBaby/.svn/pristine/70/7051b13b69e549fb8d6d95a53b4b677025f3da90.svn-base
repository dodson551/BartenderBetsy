Imports System.Threading

Module modTimer

  'm_relay(1-4) are your relay units. will be used to do things with
  Public WithEvents m_relay As DIN3Relay
  Public WithEvents m_relay2 As DIN3Relay
  Public WithEvents m_relay3 As DIN3Relay
  Public WithEvents m_relay4 As DIN3Relay

  'create a certain amount of timers, in this case, 9 total
  Dim tTimer(0 To 8) As System.Timers.Timer

  ''' <summary>
  ''' function that is referenced throughout the project. Used to do the math that is necessary to see how long the pumps need to stay on for.
  ''' </summary>
  ''' <param name="lst"></param>
  ''' <param name="tTotal"></param>
  ''' <param name="frm"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function Calculate(ByRef lst As ListView, ByVal tTotal As Integer, ByVal frm As Form)

    Dim iPart As Integer = 0
    Dim iPercent As Decimal = 0
    Dim pValue As Integer = 0
    Dim iTime As Integer = 0
    For Each i As ListViewItem In lst.Items
      iPart = iPart + i.SubItems(2).Text
    Next
    For Each i As ListViewItem In lst.Items
      iPercent = (i.SubItems(2).Text / iPart)
      iPercent = Math.Round(iPercent, 2)
      iTime = tTotal * iPercent
      Create_Timer(i.Text, iTime, frm)
    Next
    For Each i As ListViewItem In lst.Items
      tTimer(i.Text).Start()
    Next
    Return Nothing

  End Function

  ''' <summary>
  ''' function that actually creates the timer with the properties needed to turn on and off the pumps.
  ''' </summary>
  ''' <param name="iTimer"></param>
  ''' <param name="iTime"></param>
  ''' <param name="frm"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function Create_Timer(ByVal iTimer As Integer, ByVal iTime As Integer, ByVal frm As Form)

    Dim ci As New Control_Info
    ci.calling_form = frm
    ci.name = "Relay" & "" & iTimer & " : "
    ci.relays = iTimer
    tTimer(iTimer) = New System.Timers.Timer
    AddHandler tTimer(iTimer).Elapsed, AddressOf New TimerHandler(ci).OnTick
    tTimer(iTimer).Interval = iTime
    Return tTimer

  End Function

  Public Class TimerHandler

    Private m_MyInfo As Control_Info

    Sub New(ci As Control_Info)

      m_MyInfo = ci

    End Sub

    ''' <summary>
    ''' This sub sets up what the timers in this module do when they tick on their specified intervals. 
    ''' The sub detects how many relays are connected and then from there will decided what pumps need to be turned off.
    ''' The sub will then run through and then send the web request that needs to be sent to the relay unit to turn the pumps off
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub OnTick(ByVal source As Object, ByVal e As System.Timers.ElapsedEventArgs)

      Dim tt As System.Timers.Timer = source
      Dim sReq As String = "{0}outlet?"
      Dim sReq2 As String = "{0}outlet?"
      Dim sReq3 As String = "{0}outlet?"
      Dim sReq4 As String = "{0}outlet?"
      tt.Enabled = False
      RemoveHandler tt.Elapsed, AddressOf Me.OnTick

      If My.Settings.Relay_Mode = "1" Then

        sReq = sReq & "&" & m_MyInfo.relays & "={1}"
        m_relay = New DIN3Relay(My.Settings.IPAddress, My.Settings.Login, My.Settings.Password, True, 5000)
        m_relay.Connect()
        m_relay.Relay_Set_ID(sReq, "OFF")

      ElseIf My.Settings.Relay_Mode = "2 " Then

        If m_MyInfo.relays <= 8 Then
          sReq = sReq & "&" & m_MyInfo.relays & "={1}"
          m_relay = New DIN3Relay(My.Settings.IPAddress, My.Settings.Login, My.Settings.Password, True, 5000)
          m_relay.Connect()
          m_relay.Relay_Set_ID(sReq, "OFF")
        ElseIf m_MyInfo.relays >= 8 And m_MyInfo.relays <= 16 Then
          sReq2 = sReq2 & "&" & m_MyInfo.relays & "={1}"
          m_relay2 = New DIN3Relay(My.Settings.IPAddress2, My.Settings.Login2, My.Settings.Password2, True, 5000)
          m_relay2.Connect()
          m_relay2.Relay_Set_ID(sReq, "OFF")
        End If

      ElseIf My.Settings.Relay_Mode = "3" Then

        If m_MyInfo.relays <= 8 Then
          sReq = sReq & "&" & m_MyInfo.relays & "={1}"
          m_relay = New DIN3Relay(My.Settings.IPAddress, My.Settings.Login, My.Settings.Password, True, 5000)
          m_relay.Connect()
          m_relay.Relay_Set_ID(sReq, "OFF")
        ElseIf m_MyInfo.relays > 8 And m_MyInfo.relays <= 16 Then
          sReq2 = sReq2 & "&" & m_MyInfo.relays & "={1}"
          m_relay2 = New DIN3Relay(My.Settings.IPAddress2, My.Settings.Login2, My.Settings.Password2, True, 5000)
          m_relay2.Connect()
          m_relay2.Relay_Set_ID(sReq, "OFF")
        ElseIf m_MyInfo.relays > 16 And m_MyInfo.relays <= 24 Then
          sReq3 = sReq3 & "&" & m_MyInfo.relays & "={1}"
          m_relay3 = New DIN3Relay(My.Settings.IPAddress3, My.Settings.Login3, My.Settings.Password3, True, 5000)
          m_relay3.Connect()
          m_relay3.Relay_Set_ID(sReq, "OFF")
        End If

      ElseIf My.Settings.Relay_Mode = "4" Then

        If m_MyInfo.relays <= 8 Then
          sReq = sReq & "&" & m_MyInfo.relays & "={1}"
          m_relay = New DIN3Relay(My.Settings.IPAddress, My.Settings.Login, My.Settings.Password, True, 5000)
          m_relay.Connect()
          m_relay.Relay_Set_ID(sReq, "OFF")
        ElseIf m_MyInfo.relays > 8 And m_MyInfo.relays <= 16 Then
          sReq2 = sReq2 & "&" & m_MyInfo.relays & "={1}"
          m_relay2 = New DIN3Relay(My.Settings.IPAddress2, My.Settings.Login2, My.Settings.Password2, True, 5000)
          m_relay2.Connect()
          m_relay2.Relay_Set_ID(sReq, "OFF")
        ElseIf m_MyInfo.relays > 16 And m_MyInfo.relays <= 24 Then
          sReq3 = sReq3 & "&" & m_MyInfo.relays & "={1}"
          m_relay3 = New DIN3Relay(My.Settings.IPAddress3, My.Settings.Login3, My.Settings.Password3, True, 5000)
          m_relay3.Connect()
          m_relay3.Relay_Set_ID(sReq, "OFF")
        ElseIf m_MyInfo.relays > 24 And m_MyInfo.relays <= 32 Then
          sReq4 = sReq4 & "&" & m_MyInfo.relays & "={1}"
          m_relay4 = New DIN3Relay(My.Settings.IPAddress4, My.Settings.Login4, My.Settings.Password4, True, 5000)
          m_relay4.Connect()
          m_relay4.Relay_Set_ID(sReq, "OFF")
        End If

      End If

    End Sub

  End Class

  Public Class Control_Info

    Public [name] As String
    Public [calling_form] As Form
    Public relays As Integer

  End Class

End Module
