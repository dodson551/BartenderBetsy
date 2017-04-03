Imports System.IO
Imports System.IO.File
Imports System.Buffer
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions
Imports System.Reflection
#If WindowsCE = False Then
Imports System.Drawing.Imaging
Imports System.Drawing
#End If

Module modByteFunctions

#Region "Hex Conversion Stuff"


  '===============================================================================
  ' Name: baToHexString
  ' Input:
  '    varByteArray As Variant
  '    Optional AddSpaces As Boolean = True
  '    Optional lLen As Long = -1
  '    Optional lStart As Long = -1
  ' Returns: Hex String of bytes
  ' Purpose: Convert a Byte Array to a String of Hex Characters
  ' Remarks:
  '===============================================================================
  Public Function baToHexString(ByVal ba As Byte(), _
                  Optional ByVal AddSpaces As Boolean = True, _
                  Optional ByVal lLen As Long = -1, _
                  Optional ByVal lStart As Long = -1) As String

    Dim i As Integer, strBA As String
    baToHexString = ""

    If ba.Length = 0 Then Exit Function

    If lStart = -1 Then lStart = LBound(ba)
    If lLen = -1 Then lLen = UBound(ba) - LBound(ba) + 1 - lStart
    strBA = ""
    For i = lStart To lStart + lLen - 1
      If (ba(i) < 16) Then
        strBA = strBA & "0" & Hex(ba(i))
      Else
        strBA = strBA & Hex(ba(i))
      End If
      If (AddSpaces = True) Then
        strBA = strBA & " "
      End If
    Next i
    baToHexString = strBA
  End Function

  '===============================================================================
  ' Name:    Function ToHexString
  ' Purpose: Prints Hex String of byte array
  '===============================================================================
  Public Function ToHexString(ByVal bytes() As Byte) As String
    Dim hexDigits As Char() = {"0"c, "1"c, "2"c, "3"c, "4"c, "5"c, "6"c, "7"c, "8"c, "9"c, "A"c, "B"c, "C"c, "D"c, "E"c, "F"c}

    Dim chars(bytes.Length * 2) As Char

    Dim i As Integer
    For i = 0 To bytes.Length - 1
      Dim b As Integer = bytes(i)
      chars(i * 2) = hexDigits(b >> 4)
      chars(i * 2 + 1) = hexDigits(b And &HF)
    Next i
    Return New String(chars)
  End Function 'ToHexString

  '===============================================================================
  ' Name: IsValidHex
  '===============================================================================
  Function IsValidHex(ByVal strHex As String) As Boolean

    Return Regex.IsMatch(strHex, "\A\b[0-9a-fA-F]+\b\Z")

  End Function

  '===============================================================================
  ' Name: HexStringToBianry
  '===============================================================================
  Public Function HexStringToBinStr(ByVal sHex As String) As String
    If sHex.Length Mod 2 = 1 Then sHex = "0" & sHex
    Dim sBin As String = ""
    For i As Integer = 0 To sHex.Length - 1 Step 2
      Dim sHexBin As String = ConvertHexToBinary(sHex.Substring(i, 2))
      sBin += Microsoft.VisualBasic.Strings.Right("0000000" & sHexBin, 8)
    Next
    Return sBin
  End Function


  '===============================================================================
  ' Name: HexStringToByteArray
  ' Input:
  '    ByVal strHex As String
  '    byteStringValue() As Byte
  ' Returns: vbTrue or vbFalse
  ' Purpose: Converts data from a hex string (with or without spaces) to a
  '            byte array
  ' Remarks:
  '===============================================================================
  Public Function HexStringToByteArray(ByVal strHex As String) As Byte()
    Dim intArraySize As Integer
    Dim i As Integer, lLen As Long
    Dim byteStringValue() As Byte = Nothing

    On Error GoTo ExitHexString
    strHex = strHex.Replace(" ", "").Replace(vbCrLf, "")

    If Not IsValidHex(strHex) AndAlso (strHex.Length Mod 2) > 0 AndAlso strHex.Length > 0 Then
      GoTo ExitHexString
    End If

    intArraySize = strHex.Length / 2

    ReDim byteStringValue(0 To intArraySize - 1)

    For i = 0 To intArraySize - 1
      byteStringValue(i) = "&H" & Mid(strHex, i * 2 + 1, 2)
    Next i

ExitHexString:
    HexStringToByteArray = byteStringValue

  End Function

  Public Function HexToByteArray(ByVal hex As [String]) As Byte()
    Dim NumberChars As Integer = hex.Length
    Dim bytes As Byte() = New Byte(NumberChars \ 2 - 1) {}
    For i As Integer = 0 To NumberChars - 1 Step 2
      bytes(i \ 2) = Convert.ToByte(hex.Substring(i, 2), 16)
    Next
    Return bytes
  End Function

  Public Function HexStringReverseBytes(ByVal sInHex As String) As String
    If (String.IsNullOrEmpty(sInHex)) Then Return ""

    Dim ba() As Byte = HexStringToByteArray(sInHex)
    If ba Is Nothing Then Return ""
    'Reverse and return it
    Array.Reverse(ba)
    Return ByteArrayToHexString(ba, False)

  End Function
  '===============================================================================
  ' Name: baToHexString
  ' Input:
  '    varByteArray As Variant
  '    Optional AddSpaces As Boolean = True
  '    Optional lLen As Long = -1
  '    Optional lStart As Long = -1
  ' Returns: Hex String of bytes
  ' Purpose: Convert a Byte Array to a String of Hex Characters
  ' Remarks:
  '===============================================================================
  Public Function ByteArrayToHexString(ByVal ba As Byte(), _
          Optional ByVal AddSpaces As Boolean = True, _
          Optional ByVal lLen As Long = -1, _
          Optional ByVal lStart As Long = -1) As String

    Dim i As Integer, strBA As String
    ByteArrayToHexString = ""

    If ba.Length = 0 Then Return ""

    If lStart = -1 Then lStart = LBound(ba)
    If lLen = -1 Then lLen = UBound(ba) - LBound(ba) + 1 - lStart
    strBA = ""
    For i = lStart To lStart + lLen - 1
      If (ba(i) < 16) Then
        strBA = strBA & "0" & Hex(ba(i))
      Else
        strBA = strBA & Hex(ba(i))
      End If
      If (AddSpaces = True) Then
        strBA = strBA & " "
      End If
    Next i
    ByteArrayToHexString = strBA
  End Function


  '===============================================================================
  ' Name: HexStringToString
  ' Input:
  '    ByVal strHex As String
  '    ByRef sData  As String
  ' Returns: vbTrue or vbFalse
  ' Purpose: Converts data from a hex string (with or without spaces) to a
  '            byte array
  ' Remarks:
  '===============================================================================
  Public Function HexStringToString(ByVal strHex As String, ByRef sData As String) As Boolean
    Dim intArraySize As Integer
    Dim i As Integer, iLen As Integer
    HexStringToString = True
    On Error GoTo ExitHexString
    strHex = strHex.Replace(" ", "")
    strHex = strHex.Replace(vbCrLf, "")
    sData = ""

    iLen = Len(strHex)
    If (iLen Mod 2) > 0 Then
      iLen = iLen + 1
      strHex = "0" & strHex
    End If
    intArraySize = CInt(iLen / 2)

    If (intArraySize < 1) Then
      Exit Function
    End If

    For i = 0 To intArraySize - 1
      sData = sData & Chr(CInt("&H" & Mid(strHex, i * 2 + 1, 2)))
    Next i

    Exit Function
ExitHexString:
    HexStringToString = False

  End Function

#End Region      ' "Hex Conversion Stuff"

#Region "Mimic VB6 Functionality"
  '===============================================================================
  ' Name:    Function Left
  ' Purpose: Mimic VB6 function
  '===============================================================================
  Public Function Left(ByVal str As String, ByVal index As Integer) As String
    If (str.Length < index - 1) Then
      index = str.Length
    End If

    Left = str.Substring(0, index)
  End Function

  '===============================================================================
  ' Name:    Function Right
  ' Purpose: Mimic VB6 function
  '===============================================================================
  Public Function Right(ByVal str As String, ByVal index As Integer) As String
    Right = str.Substring(str.Length - index)
  End Function
#End Region    '"Mimic VB6 Functionality"

  '===============================================================================
  ' Name:    Function ConvertStringToByteArray
  '===============================================================================
  Public Function ConvertStringToByteArray(ByVal stringToConvert As String) As Byte()
    Return System.Text.Encoding.ASCII.GetBytes(stringToConvert.ToCharArray())
  End Function



  '===============================================================================
  ' Name:    Function GetBytes
  ' Purpose: Get bytes from a hex string into a byte array
  '===============================================================================
  Public Function GetBytes(ByVal hexString As String, Optional ByVal bReverse As Boolean = False) As Byte()
    hexString = hexString.Replace(vbCrLf, "").Trim
    Dim byteLength As Integer = hexString.Length / 2
    Dim bytes(0 To byteLength - 1) As Byte
    Dim hex As String
    Dim j As Integer = 0
    Dim i As Integer
    'Changed from -2 to -1 for length one strings
    For i = 0 To bytes.Length - 1
      If (bReverse) Then
        hex = New [String](New [Char]() {hexString(byteLength * 2 - (j + 2)), hexString((byteLength * 2 - (j + 1)))})
      Else
        hex = New [String](New [Char]() {hexString(j), hexString((j + 1))})
      End If

      bytes(i) = CByte("&H" & hex)
      j = j + 2
    Next i
    Return bytes
  End Function 'GetBytes


  '===============================================================================
  ' Name: baInfo
  ' Input:
  '    ByRef ba() As Byte
  ' Returns: String with length and bytes of array
  ' Remarks: made for .Net compatibility
  '===============================================================================
  Public Function baInfo(ByVal ba() As Byte) As String
    Dim strBytes As String, iLoop As Integer
    strBytes = "(" & CStr(UBound(ba) + 1) & ") "
    For iLoop = LBound(ba) To UBound(ba)
      strBytes = strBytes & CInt(ba(iLoop)) & " "
    Next
    baInfo = strBytes
  End Function
  '===============================================================================
  ' Name: strToByteArray
  ' Input:
  '    ByRef varArrayToWorkOn as variant
  '    ByVal lNumBytes As long
  ' Returns:
  ' Purpose: Convert a String to a Byte Array
  ' Remarks: Made for .Net compatibility
  '===============================================================================
  Public Function strToByteArray(ByVal StringIn$) As Byte()
    Dim lLoop As Long, ByteArray() As Byte

    ReDim ByteArray(0 To Len(StringIn$) - 1)
    If (Len(StringIn$) - 1 > UBound(ByteArray)) Then
      Return Nothing
    End If

    For lLoop = 0 To Len(StringIn$) - 1
      ByteArray(lLoop) = Asc(Mid(StringIn$, lLoop + 1))
    Next
    Return ByteArray
  End Function

  '===============================================================================
  ' Name: baToStr
  ' Input:
  '    ByRef varArrayToWorkOn as variant
  '    ByVal lNumBytes As long
  ' Returns:
  ' Purpose: Convert a Byte array into a String
  ' Remarks: Made for .Net compatibilitiy
  '===============================================================================
  Public Function baToStr(ByVal ByteArray() As Byte, _
                              Optional ByVal iLen As Integer = -1, _
                              Optional ByVal iStart As Integer = -1) As String
    Dim iLoop As Integer, MyStr As String = ""
    If ByteArray Is Nothing Then
      Return ""
    End If
    If iStart = -1 Then iStart = LBound(ByteArray)
    If iLen = -1 Then iLen = UBound(ByteArray) - LBound(ByteArray) + 1
    For iLoop = iStart To iStart + iLen - 1
      MyStr = MyStr & Chr(ByteArray(iLoop))
    Next iLoop
    baToStr = MyStr
  End Function

  Public Function ByteArrayToString(ByVal ba() As Byte, Optional ByVal iStart As Integer = 0, Optional ByVal iLength As Integer = -1) As String
    If iLength < 1 Then
      iLength = ba.Length - iStart
    End If
    Return Encoding.ASCII.GetString(ba, iStart, iLength)
  End Function

  Public Function StringToByteArray(ByVal sData As String) As Byte()
    'System.Text.UnicodeEncoding.Unicode.GetBytes(sData
    Return System.Text.UnicodeEncoding.Unicode.GetBytes(sData)
  End Function

  '===============================================================================
  ' Name: fillString
  '===============================================================================
  Public Function fillString(ByVal str As String, Optional ByVal iLen As Integer = 32, Optional ByVal iChar As Integer = 0) As String
    Return Left(str & StrDup(iLen, Chr(iChar)), iLen)
  End Function

  '===============================================================================
  ' Name: Sub AddSpaces
  ' Purpose: Insert spaces into a hex string to make it more readable.
  '===============================================================================
  Public Function AddSpacesToHexString(ByRef strText As String) As String
    Dim i As Short

    i = 3
    Do While (i <= Len(strText))
      strText = Left(strText, i - 1) & " " & Right(strText, Len(strText) - (i - 1))
      i = i + 3
    Loop

    AddSpacesToHexString = strText
  End Function

  '===============================================================================
  ' Name: StringToByteArray_2
  ' Input:
  '    ByRef varArrayToWorkOn as variant
  '    ByVal lNumBytes As long
  ' Returns:
  ' Purpose: Convert a String to a Byte Array
  ' Remarks:
  '===============================================================================
  Public Sub StringToByteArray_2(ByVal StringIn$, ByRef ByteArray() As Byte)
    Dim i%

    ReDim ByteArray(0 To Len(StringIn$) - 1)
    If (Len(StringIn$) - 1 > UBound(ByteArray)) Then
      'MsgBox ("Destination Array in String2ByteArray is not large enough!")
      Exit Sub
    End If

    For i = 0 To Len(StringIn$) - 1
      ByteArray(i) = Asc(Mid(StringIn$, i + 1))
    Next i

  End Sub

  '===============================================================================
  ' Name: strAsHexString
  ' Input:
  '    ByRef varArrayToWorkOn as variant
  '    ByVal lNumBytes As long
  ' Returns:
  ' Purpose: Convert a String to a Byte Array
  ' Remarks:
  '===============================================================================
  Public Function strAsHexString(ByVal strData As String) As String
    On Error Resume Next
    Dim i As Integer
    Dim sHex As String = "", tmpAppend As String
    For i = 1 To Len(strData)
      tmpAppend = Hex(Asc(Mid(strData, i, 1)))
      If tmpAppend.Length = 1 Then tmpAppend = "0" & tmpAppend
      sHex = sHex & tmpAppend
    Next
    strAsHexString = sHex
  End Function

  '===============================================================================
  ' Name:    Function Hex2Str
  ' Purpose: Turn a hex string into a string
  ' Remarks: Used for AES_Rijndael Block Cipher
  '===============================================================================
  Public Function Hex2Str(ByVal sHex As String) As String
    On Error Resume Next
    Dim i As Integer
    Dim sDecoded As String = "", tmpChar As String
    For i = 1 To Len(sHex) Step 2
      sDecoded = sDecoded & Chr(Val("&H" & Mid(sHex, i, 2)))
    Next i
    Hex2Str = sDecoded
  End Function

  '===============================================================================
  ' Name:    Function Str2Hex
  ' Purpose: Turn a string into a hex string
  ' Remarks: Used for AES_Rijndael Block Cipher
  '===============================================================================
  Public Function Str2Hex(ByVal strData As String) As String
    On Error Resume Next
    Dim i As Integer
    Dim sHex As String = "", tmpAppend As String
    For i = 1 To Len(strData)
      tmpAppend = Hex(Asc(Mid(strData, i, 1)))
      If tmpAppend.Length = 1 Then tmpAppend = "0" & tmpAppend
      sHex = sHex & tmpAppend
    Next
    Str2Hex = sHex
  End Function

  Private Function streamToString(ByVal stream As System.IO.MemoryStream) As String
    Dim o As New System.IO.StreamWriter(stream, System.Text.Encoding.Default)
    Dim bytes(stream.Length - 1) As Byte
    stream.Read(bytes, 0, stream.Length - 1)
    Return Encoding.ASCII.GetString(bytes, 0, UBound(bytes))
  End Function

  Public Function StrClone(ByVal Source As String, ByVal Count As Integer) As String
    Return New String(" "c, Count).Replace(" ", Source) ' Replace(Space(Count), " ", Source)
  End Function

  Public Function removeCharsFromEndOfString(ByVal StringValue As String, ByVal removedCharacters As Integer) As String
    Return StringValue.Substring(0, StringValue.Length - removedCharacters)
  End Function



  Public Sub LongToByteArray(ByRef pDst() As Byte, ByVal iOffest As Integer, ByVal iLongVal As Long)

    'Dim ba() As Byte = {0, 2, 4, 8}

    'Create Gchandle instance and pin variable required
    Dim MyGC As GCHandle = GCHandle.Alloc(iLongVal, GCHandleType.Pinned)

    'get address of variable in pointer variable
    Dim AddofLongValue As IntPtr = MyGC.AddrOfPinnedObject()

    'Use copy method to copy array data to variable’s address with length specified(4)
    Marshal.Copy(AddofLongValue, pDst, iOffest, 4)

    'First read value of variable from its address    'in memory in order to use it
    iLongVal = Marshal.ReadInt32(AddofLongValue)

    'Free GChandle to avoid memory leaks
    MyGC.Free()

    'Debug.Print("%02x %02x %02x %02x ", pDst(0), pDst(1), pDst(2), pDst(3))

  End Sub 'LongToByteArray 

  Public Sub ByteArrayToLong(ByRef iLongVal As Int32, ByVal pSrc() As Byte, ByVal iOffest As Integer)

    'Create Gchandle instance and pin variable required
    Dim MyGC As GCHandle = GCHandle.Alloc(iLongVal, GCHandleType.Pinned)

    'get address of variable in pointer variable
    Dim AddofLongValue As IntPtr = MyGC.AddrOfPinnedObject()

    'Use copy method to copy array data to variable’s address with length specified(4)
    Marshal.Copy(pSrc, iOffest, AddofLongValue, 4)

    'First read value of variable from its address    'in memory in order to use it
    iLongVal = Marshal.ReadInt32(AddofLongValue)

    'Free GChandle to avoid memory leaks
    MyGC.Free()

  End Sub 'ByteArrayToLong           

  '===============================================================================
  ' Name: d2b
  ' Input:
  '    MyDecimal As Variant
  '    Optional iMinLen As Integer = -1
  ' Returns: Binary string of decimal value
  ' Purpose: Decimal to Binary Conversion Function
  '===============================================================================
  Public Function d2b(ByVal MyDecimal As Object, Optional ByVal iMinLen As Integer = -1) As String
    Dim NumBits As Object, NumBytes As Object, MyLeftOver As Object, iBit%
    Dim OldTopBit As Integer, MyBin As String = ""

    If iMinLen = 0 Then Return ""
    If MyDecimal = 0 Then GoTo endFunc
    MyBin = ""
    NumBits = (Math.Log(MyDecimal) / Math.Log(2))
    NumBits = Int(NumBits) + 1
    NumBytes = NumBits / 8
    If (NumBytes - Int(NumBytes) > 0) Then NumBytes = Int(NumBytes) + 1
    MyLeftOver = CDec(MyDecimal)
    For iBit = NumBits To 1 Step -1
      If (MyLeftOver / (2 ^ (iBit - 1))) >= 1 Then
        MyBin = MyBin & "1"
        OldTopBit = 1
      Else
        MyBin = MyBin & "0"
        OldTopBit = 0
      End If
      MyLeftOver = MyLeftOver - OldTopBit * (2 ^ (iBit - 1))
    Next iBit

    If (iMinLen > Len(MyBin)) Then
endFunc:
      Do
        MyBin = "0" & MyBin
      Loop While (Len(MyBin) < iMinLen)
    End If
    d2b = MyBin
  End Function

  '===============================================================================
  ' Name: b2d
  ' Input:
  '    MyBinary As String
  ' Returns:
  '    Variant with decimal value of binary string
  ' Purpose: Binary to Decimal Conversion Function
  ' Remarks:
  '===============================================================================
  Public Function b2d(ByVal MyBinary As String) As Long
    Dim i%, LenBin%, BITVAL%
    Dim retval As Long = 0
    LenBin = Len(MyBinary)
    For i = 0 To (LenBin - 1)
      BITVAL = CInt(Mid(MyBinary, (LenBin - i), 1))
      retval = retval + BITVAL * (2 ^ i)
    Next i
    Return retval
  End Function


  ' only good for stings with length < 255
  Public Function BinaryStringToHexArray(ByVal sBinary As String, Optional ByVal bMinLength As Boolean = 2) As String
    Dim ba(-1) As Byte, iNumBits As Integer = 0, sRet As String
    Dim sHex As String, sHexLen As String

    BinaryStringToByteArray(sBinary, ba, iNumBits)
    sHex = ByteArrayToHexString(ba, False)
    If sHex.Length < bMinLength Then
      sHex = StrDup(bMinLength - sHex.Length, "0") & sHex
    End If

    sHexLen = Hex(iNumBits)
    If sHexLen.Length = 1 Then sHexLen = "0" & sHexLen
    sRet = "0x" & sHexLen & sHex

    Return sRet

  End Function

  ' only good for stings with length < 255
  Public Function BinaryStringToHexString(ByVal sBinary As String, _
                                          Optional ByVal bMinLength As Boolean = 2, _
                                          Optional ByVal bInclude0x As Boolean = True, _
                                          Optional ByVal bPrependHexLength As Boolean = True, _
                                          Optional ByVal bRevBytes As Boolean = True) As String
    Dim ba(-1) As Byte, iNumBits As Integer = 0
    Dim sHex As String = ""

    BinaryStringToByteArray(sBinary, ba, iNumBits, bRevBytes)
    sHex = ByteArrayToHexString(ba, False)

    ' Use a longer length if required
    If sHex.Length < bMinLength Then
      sHex = StrDup(bMinLength - sHex.Length, "0") & sHex
    End If

    ' Append the 0x if required
    If bPrependHexLength Then
      Dim sHexLen As String = Hex(iNumBits)
      If sHexLen.Length Mod 2 = 1 Then sHexLen = "0" & sHexLen
      sHex = sHexLen & sHex
    End If

    If bInclude0x Then sHex = "0x" & sHex

    Return sHex

  End Function


  '===============================================================================
  ' Name: GetParity
  ' Input:
  '    ByVal MyBinary As String - Binary String to calculate parity for  
  '    ByVal bEvenParity as Boolean (True is Even Parity, False is Odd Parity)
  ' Returns: 0 or 1 as string
  ' Purpose: Get Bit Value
  ' Remarks:
  '===============================================================================
  Public Function GetParity(ByVal MyBinary As String, ByVal bEvenParity As Boolean) As String

    Dim iCount As Integer = 0

    For Each c As Char In MyBinary
      If (c = "1") Then
        iCount += 1
      End If
    Next

    Dim bRet As Boolean
    If ((iCount Mod 2) = 0) Then
      bRet = bEvenParity
    Else
      bRet = Not bEvenParity
    End If

    If (bRet) Then
      Return "1"
    Else
      Return "0"
    End If
  End Function

  Public Sub BinaryStringToByteArray(ByVal sBinary As String, _
                                   ByRef ba() As Byte, _
                                   ByRef iNumBits As Integer, _
                                   Optional ByVal bRev As Boolean = True)
    Dim j As Integer
    Dim iSizeArray As Integer
    ' Get rid of any spaces
    sBinary = sBinary.Trim

    ' Get the number of bits
    iNumBits = sBinary.Trim.Length

    ' If the number of bits mod 8 is 
    If iNumBits Mod 8 <> 0 Then
      sBinary = StrDup(8 - (iNumBits Mod 8), "0") & sBinary
    End If

    ' Calculate the size
    iSizeArray = (sBinary.Length \ 8)
    ReDim ba(0 To iSizeArray - 1)

    If (bRev) Then

      j = iSizeArray
      For i As Integer = sBinary.Length - 8 To 0 Step -8
        j = j - 1
        ba(j) = b2d(sBinary.Substring(i, 8))
      Next
    Else
      j = iSizeArray
      For i As Integer = 0 To sBinary.Length - 8 Step 8
        j = j - 1
        ba(j) = b2d(sBinary.Substring(i, 8))
      Next
    End If

  End Sub

  '===============================================================================
  ' Name: BinaryToHex
  ' Input:
  '    sBin As String
  ' Returns:
  '    Variant with decimal value of binary string
  ' Purpose: Binary to Decimal Conversion Function
  ' Remarks:
  '===============================================================================
  Public Function BinaryToHex(ByVal sBin As String, Optional ByVal iLen As Integer = 56) As String
    Dim i%
    Dim sHexString As String = ""
    sBin = sBin.PadLeft(iLen, "0")
    Dim asBin() As String, asHex() As String
    ReDim asBin(0 To (iLen / 8) - 1)
    ReDim asHex(0 To (iLen / 8) - 1)

    For i = 0 To (iLen / 8) - 1 Step 1
      asBin(i) = sBin.Substring(i * 8, 8)
      asHex(i) = Right("0" & Hex(b2d(asBin(i))), 2)
      sHexString = sHexString & asHex(i)
    Next

    Return sHexString
  End Function

  '===============================================================================
  ' Name: GetBit
  ' Input:
  '    ByVal MyByte As Integer
  '    ByVal Bit As Integer
  ' Returns: 0 or 1
  ' Purpose: Get Bit Value
  ' Remarks:
  '===============================================================================
  Public Function GetBit(ByVal MyByte As Integer, ByVal Bit As Integer) As Integer
    GetBit = MyByte \ (2 ^ Bit)
    GetBit = GetBit Mod 2
  End Function

  '===============================================================================
  ' Name: HexStr
  ' Input:
  '           vNumberToConvert - number
  '           lNumberHexDigits - size of hex string
  ' Output:
  '           none
  '
  ' Returns:  Hexidecimal string
  '===============================================================================
  Public Function HexStr(ByVal vNumberToConvert As Object, _
                         Optional ByVal lNumberHexDigits As Long = 8) As String
    On Error GoTo HexStrErrorHandler
    vNumberToConvert = CLng(Val(vNumberToConvert))
    If False Then
HexStrErrorHandler:
      vNumberToConvert = 0
    End If
    HexStr = Hex(vNumberToConvert)
    While (Len(HexStr) < lNumberHexDigits)
      HexStr = "0" & HexStr
    End While
  End Function


  '===============================================================================
  ' Name: StrToHexStr
  ' Input:
  '    strOriginal As String
  '    Optional AddSpaces As Boolean = True
  ' Returns: Hex String of bytes
  ' Purpose: Convert a String to a String of Hex Characters
  '===============================================================================
  Public Function StrToHexStr_1(ByVal strOriginal As String, _
                              Optional ByVal AddSpaces As Boolean = True) As String
    Dim i As Long
    Dim tmp$ = "", MyStr$ = ""
    For i = 1 To Len(strOriginal)
      tmp$ = Hex$(Asc(Mid(strOriginal, i, 1)))
      If Len(tmp$) = 1 Then tmp$ = "0" & tmp$
      MyStr$ = MyStr$ & tmp$
      If AddSpaces Then MyStr$ = MyStr$ & " "
    Next i
    StrToHexStr_1 = MyStr$
  End Function


  Public Function StrToHexStr(ByVal strOriginal As String, _
                              Optional ByVal AddSpaces As Boolean = True) As String
    Dim byteArray() As Byte
    Dim hexNumbers As System.Text.StringBuilder = New System.Text.StringBuilder

    byteArray = System.Text.ASCIIEncoding.ASCII.GetBytes(strOriginal)

    For i As Integer = 0 To byteArray.Length - 1
      hexNumbers.Append(byteArray(i).ToString("xx"))
    Next
    Return hexNumbers.ToString
  End Function

  '===============================================================================
  ' Name: MakeHexDisplayString
  ' Input:
  '    varData As Variant
  '    Optional lStart As Long = 0
  '    Optional lLength = -1
  ' Returns: HexDisplayString of data
  ' Purpose: Returns data with both hex values and ascii values
  ' Remarks:
  '===============================================================================
  Public Function MakeHexDisplayString(ByVal baData As Byte(), _
                                      Optional ByVal iStart As Integer = 0, _
                                      Optional ByVal iLength As Integer = -1, _
                                      Optional ByVal boolShowAscii As Boolean = False, _
                                      Optional ByVal boolDisplayToSixteen As Boolean = False, _
                                      Optional ByVal sFillChar As String = ".") As String

    Dim CharStr As String = ""
    Dim AscStr As String = ""
    Dim HexStr As String = ""
    Dim FullStr As String = ""
    Dim iLoop As Integer
    Dim ByteArray() As Byte, boolFinishedOnSixteen As Boolean
    Dim iXtraBytesNeeded As Integer

    MakeHexDisplayString = ""
    If (baData.Length = 0) Then Exit Function

    iXtraBytesNeeded = 0
    FullStr$ = ""

    ' Set length if not defined
    If iLength = -1 Then
      ' subtract start because it can only be as long as varData has bytes
      iLength = baData.Length - iStart
    End If

    If (iLength <= 0) Or (iStart < 0) Then Exit Function

    ' Constrain lLength to be within the array bounds
    iLength = Math.Min(baData.Length - iStart, iLength)

    ReDim ByteArray(0 To iLength - 1)

    Array.Copy(baData, iStart, ByteArray, 0, iLength)

    ' If needed adjust Length so that a full 16 bytes are displayed per line
    If (boolDisplayToSixteen) Then
      iXtraBytesNeeded = 16 - (iLength Mod 16)
      If (iXtraBytesNeeded = 16) Then iXtraBytesNeeded = 0
      iLength = iLength + iXtraBytesNeeded
      ' Grow the array to fit the extra bytes
      ReDim Preserve ByteArray(0 To iLength - 1)
    End If

    For iLoop = 0 To iLength - 1
      CharStr = Hex$(ByteArray(iLoop))
      If ByteArray(iLoop) < 16 Then CharStr = "0" & CharStr
      HexStr = HexStr & CharStr & " "

      If (ByteArray(iLoop) > &H7E) Or (ByteArray(iLoop) < &H20) Then
        AscStr = AscStr & sFillChar
      Else
        AscStr = AscStr & Chr(CInt(ByteArray(iLoop)))
      End If

      boolFinishedOnSixteen = False

      If ((iLoop + 1) Mod 16 = 0) Then
        boolFinishedOnSixteen = True
        If boolShowAscii Then
          FullStr$ = FullStr$ & HexStr$ & " | " & AscStr$ & vbCrLf
        Else
          FullStr$ = FullStr$ & HexStr$ & vbCrLf
        End If

        HexStr = ""
        AscStr = ""
      End If
    Next iLoop

    If (boolFinishedOnSixteen = False) Then
      If boolShowAscii Then
        iXtraBytesNeeded = 16 - (iLength Mod 16)
        If (iXtraBytesNeeded = 16) Then iXtraBytesNeeded = 0
        FullStr$ = FullStr$ & HexStr$ & StrDup((iXtraBytesNeeded * 3), " ") & " | " & AscStr$ & vbCrLf
      Else
        FullStr$ = FullStr$ & HexStr$ & vbCrLf
      End If
      HexStr = ""
      AscStr = ""
    End If

    MakeHexDisplayString = FullStr$

  End Function

  'Application Path - Returns an ending "\"
  'Public Function GetAppPath() As String
  '  Return System.AppDomain.CurrentDomain.BaseDirectory()
  'End Function

  'Application Path - Returns an ending "\"
#If Not WindowsCE And Not PocketPC Then
  Public Function Application_Data_Path() As String
    Return System.AppDomain.CurrentDomain.BaseDirectory()
  End Function
#End If

  'Takes a hex character string and returns the binary representation in string format. 
  'Pads each hex character's representation to 4 places with leading zero's.
  Public Function ConvertHexToBinary(ByVal sHex As String) As String
    Dim intCounter As Integer
    Dim aChar() As Char = sHex.Replace(" ", "").ToCharArray
    Dim sBinary As String = ""
    Dim cTemp As Char
    Dim iTemp As Int32

    'Iterate through the string.
    For intCounter = 0 To aChar.Length - 1
      cTemp = aChar(intCounter)
      'Convert hex character to Int.
      iTemp = Convert.ToInt32(cTemp, 16)
      'Convert int to string using base2 and pad with zeros.
      sBinary &= Convert.ToString(iTemp, 2).PadLeft(4, "0")
    Next

    Return sBinary
  End Function

  'Checks that the sHexData string contains only valid hex characters
  Public Function validateHexData(ByVal sHexData As String) As Boolean
    Return (sHexData.Length < 120) AndAlso Regex.IsMatch(sHexData, "^[0-9A-F]+$")
  End Function

  Public Function convertToBinary(ByVal sHex As String, ByVal iBits As Integer) As String
    Dim temp As String = ConvertHexToBinary(sHex)
    Return temp.Substring(temp.Length - iBits)
    ' Does the same thing - verified
    '    Return Microsoft.VisualBasic.Right(ConvertHexToBinary(sHex), iBits)
  End Function

  Public Function ShaveNullOffFrontOfHexString(ByVal sBadge As String) As String
    Dim sNullHex As String = "0"
    Dim iLast As Integer

    For i As Integer = 0 To sBadge.Length - 1 Step 1
      If Not sNullHex = sBadge.Substring(i, 1) Then
        iLast = i
        Exit For
      End If
    Next

    sBadge = sBadge.Substring(iLast)
    Return sBadge
  End Function

#Region "CamelCase"

  Private Enum [Case]
    PascalCase
    CamelCase
  End Enum

  ''' <summary>
  ''' Converts the phrase to specified convention.
  ''' </summary>
  ''' <param name="phrase"></param>
  ''' <param name="cases">The cases.</param>
  ''' <returns>string</returns>
  Private Function ConvertCaseString(ByVal phrase As String, ByVal cases As [Case]) As String
    Dim splittedPhrase As String() = phrase.Split(" "c, "-"c, "."c)
    Dim sb As StringBuilder = New StringBuilder()

    If cases = [Case].CamelCase Then
      sb.Append(splittedPhrase(0).ToLower())
      splittedPhrase(0) = String.Empty
    ElseIf cases = [Case].PascalCase Then
      sb = New StringBuilder()
    End If

    For Each s As [String] In splittedPhrase
      Dim splittedPhraseChars As Char() = s.ToCharArray()
      If splittedPhraseChars.Length > 0 Then
        splittedPhraseChars(0) = ((New [String](splittedPhraseChars(0), 1)).ToUpper().ToCharArray())(0)
      End If
      sb.Append(New [String](splittedPhraseChars))
    Next
    Return sb.ToString()
  End Function

  '===============================================================================
  ' Name:    PascalCaseWords
  ' Purpose: Capitalize only the first letter of each word
  '===============================================================================
  Public Function PascalCaseWords(ByVal sText As String) As String
    Dim sRet As String = ""
    Dim sWords() As String = Split(sText)

    For Each word As String In sWords
      If word.Length > 0 Then
        sRet &= word.Substring(0, 1).ToUpper & word.Substring(1, word.Length - 1).ToLower & " "
      End If
    Next

    Return sRet.Trim
  End Function

  Public Function ValidateString(ByVal str As String, _
                             Optional ByVal sMatch As String = "A-Fa-f0-9") As Boolean

    Dim pattern As String = "^[" & sMatch & "]*$" '= "^[a-zA-Z\s]+$"
    Dim reg As New Regex(pattern)
    Return reg.IsMatch(str)
  End Function

  Public Function CleanStringToAscii(ByVal str As String) As String
    str = Regex.Replace(str, "[^\u0022-\u007E]", String.Empty) 'force ASCII
    Return str
  End Function

#End Region

#Region "File Read And Write Function"
#If Not WindowsCE And Not PocketPC Then
  '===============================================================================
  ' Name: Function ByteArrayToFile
  ' Input:
  '    ByVal strFilename As String
  '    ByRef ByteArray() As Byte
  ' Output:
  '    Boolean - boolean value indicating whether read was successful
  ' Purpose: Write Byte Array into File
  ' Remarks:
  '===============================================================================
  Public Function ByteArrayToFile(ByVal strFilename As String, ByVal ByteArray() As Byte, _
  Optional ByVal EraseOriginal As Boolean = False, _
  Optional ByVal lBytes As Long = 0) As Boolean

    Dim lLen As Long, iFile As Integer, baTemp() As Byte

    ByteArrayToFile = False
    If (EraseOriginal) Then
      On Error Resume Next
      Kill(strFilename)
      On Error GoTo 0
    End If
    On Error GoTo errorhandler

    ' Write to disk
    iFile = FreeFile()
    FileOpen(iFile, strFilename, OpenMode.Binary, OpenAccess.Write)
    If (lBytes <= 0) Then
      FilePut(iFile, ByteArray)
    Else
      ReDim baTemp(0 To lBytes - 1)
      'Call CopyMemory(baTemp(0), ByteArray(0), lBytes)
      Buffer.BlockCopy(baTemp, 0, ByteArray, 0, lBytes)
      FilePut(iFile, baTemp)
    End If
    ByteArrayToFile = True
errorhandler:
    On Error Resume Next
    FileClose(iFile)

  End Function

  '===============================================================================
  ' Name: Function FileToByteArray
  ' Input:
  '    ByVal strFilename As String
  '    ByRef ByteArray() As Byte
  ' Output:
  '    Boolean - boolean value indicating whether read was successful
  ' Purpose: Read File into Byte Array
  ' Remarks:
  '===============================================================================
  Public Function FileToByteArray(ByVal strFilename As String, ByRef ByteArray() As Byte, Optional ByRef lFileSize As Integer = 0) As Boolean
    Dim iFile As Short
    Dim baTemp() As Byte
    Dim lLen As Integer

    On Error GoTo ExitFileToByteArray
    FileToByteArray = False
    lFileSize = FileLen(strFilename)
    If (lFileSize = 0) Then GoTo ExitFileToByteArray

    'Read from disk
    iFile = FreeFile()
    FileOpen(iFile, strFilename, OpenMode.Binary, OpenAccess.Read)
    ReDim baTemp(lFileSize - 1)
    Dim sa As System.Array = Nothing
    FileGet(iFile, sa)
    baTemp = sa
    FileClose(iFile)

    On Error GoTo RedimFailed
    ReDim ByteArray(lFileSize - 1)
    If (False) Then
RedimFailed:
      On Error GoTo ExitFileToByteArray
      If ((UBound(ByteArray) - LBound(ByteArray) + 1) < lFileSize) Then
        GoTo ExitFileToByteArray
      End If
    End If
    Buffer.BlockCopy(baTemp, 0, ByteArray, 0, lFileSize)
    FileToByteArray = True
ExitFileToByteArray:
    Exit Function
    MsgBox("File " & strFilename & " Not Found")

  End Function
#End If

  Public Function FileIntoByteArray(ByVal sFile As String) As Byte()
    ' Open a file that is to be loaded into a byte array
    Dim oFile As System.IO.FileInfo
    oFile = New System.IO.FileInfo(sFile)

    Dim oFileStream As System.IO.FileStream = oFile.OpenRead()
    Dim lBytes As Long = oFileStream.Length
    Dim fileData(lBytes - 1) As Byte

    If (lBytes > 0) Then
      ' Read the file into a byte array
      oFileStream.Read(fileData, 0, lBytes)
    End If
    Try
      oFileStream.Close()
    Catch ex As Exception

    End Try

    Return fileData

  End Function

#If Not WindowsCE And Not PocketPC Then
  Public Function ConvertIconToByteArray(ByVal img As Icon) As Byte()
    Dim baImg() As Byte = Nothing

    Try
      Using ms As New MemoryStream()
        img.Save(ms)
        baImg = ms.ToArray()
      End Using
      Return baImg

    Catch ex As Exception
      Return Nothing
    End Try
  End Function
#End If

#End Region

  Public Function GetStreamAsByteArray(ByVal stream As MemoryStream) As Byte()

    Dim streamLength As Integer = Convert.ToInt32(stream.Length)

    Dim fileData As Byte() = New Byte(streamLength) {}

    ' Read the file into a byte array
    stream.Read(fileData, 0, streamLength)
    stream.Flush()
    stream.Close()

    Return fileData

  End Function


End Module
