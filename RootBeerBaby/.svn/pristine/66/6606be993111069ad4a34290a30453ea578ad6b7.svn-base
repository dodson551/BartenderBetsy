Imports System
Imports System.ComponentModel
Imports System.Net

Public Class WebClientWithTimeout
  Inherits WebClient

  Private m_iTimeoutMs As Integer
  ''' <summary>
  ''' Time in milliseconds
  ''' </summary>
  Public Property TimeoutMs() As Integer
    Get
      Return m_iTimeoutMs
    End Get
    Set(value As Integer)
      m_iTimeoutMs = value
    End Set
  End Property

  Public Sub New()
    Me.m_iTimeoutMs = 20000
  End Sub

  Public Sub New(iTimeoutMs As Integer)
    Me.m_iTimeoutMs = iTimeoutMs
    Debug.Print("WebClientWithTimeout, Timeout = " & iTimeoutMs)
  End Sub

  Protected Overrides Function GetWebRequest(address As Uri) As WebRequest
    Dim result = MyBase.GetWebRequest(address)
    result.Timeout = Me.m_iTimeoutMs
    Return (result)
  End Function

End Class