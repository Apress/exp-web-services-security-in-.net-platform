Option Strict Off
Option Explicit On
Module basConvert


    '****************************************
    'CONVERTED TO .NET by TODD ACHESON,  todd@acheson.com
    'January 2003
    'The .NET version definitely could be streamlined and 
    'cleaned up a bit to be more efficient.  Used the upgrade wizard
    'and tinkered with it a bit myself.  S and P box arrays are the 
    'same as the VB6 version
    'Added ability to crypt one file, all files in a folder, and
    'all files/folders in a one folder recursively.
    '*****************************************


	' basConvert: Utilities to convert between byte arrays, hex strings,
	' strings containing binary values, and 32-bit word arrays.
	
	' Version 1. Published January 2002
	'************************* COPYRIGHT NOTICE*************************
	' This code was originally written in Visual Basic by David Ireland
	' and is copyright (c) 2000-2 D.I. Management Services Pty Limited,
	' all rights reserved.
	
	' You are free to use this code as part of your own applications
	' provided you keep this copyright notice intact.
	
	' This code may only be used as part of an application. It may
	' not be reproduced or distributed separately by any means without
	' the express written permission of the author.
	
	' David Ireland and DI Management Services Pty Limited make no
	' representations concerning either the merchantability of this
	' software or the suitability of this software for any particular
	' purpose. It is provided "as is" without express or implied
	' warranty of any kind.
	
	' Please forward comments or bug reports to <code@di-mgt.com.au>.
	' The latest version of this source code can be downloaded from
	' www.di-mgt.com.au/crypto.html.
	'****************** END OF COPYRIGHT NOTICE*************************
	
	' The Public Functions in this module are:
	' cv_BytesFromHex(sInputHex): Returns array of bytes
	' cv_BytesFromString(strInput): Returns array of bytes
	' cv_WordsFromHex(sHex): Returns array of words (Longs)
	' cv_HexFromWords(aWords): Returns hex string
	' cv_HexFromBytes(aBytes()): Returns hex string
	' cv_HexFromString(str): Returns hex string
	' cv_StringFromHex(strHex): Returns string of ascii characters
	' cv_GetHexByte(sInputHex, iIndex): Extracts iIndex'th byte from hex string
	' RandHexByte(): Returns random byte as a 2-digit hex string
	
	Public Function cv_BytesFromHex(ByVal sInputHex As String) As Object
		' Returns array of bytes from hex string in big-endian order
		' E.g. sHex="FEDC80" will return array {&HFE, &HDC, &H80}
        Dim i As Long
        Dim M As Long
		Dim aBytes() As Byte
		If Len(sInputHex) Mod 2 <> 0 Then
			sInputHex = "0" & sInputHex
		End If
		
		M = Len(sInputHex) \ 2
		ReDim aBytes(M - 1)
		
		For i = 0 To M - 1
			aBytes(i) = Val("&H" & Mid(sInputHex, i * 2 + 1, 2))
		Next 
		
		cv_BytesFromHex = VB6.CopyArray(aBytes)
        Return cv_BytesFromHex
	End Function
	
    Public Function cv_BytesFromString(ByRef strInput As String) As Byte()
        ' Converts string <strInput> of ascii chars to array of bytes
        ' str may contain chars of any value between 0 and 255.
        ' E.g. strInput="abc." will return array {&H61, &H62, &H63, &H2E}
        Dim aBytes() As Byte
        aBytes = System.Text.Encoding.Default.GetBytes(strInput)
        Return aBytes
    End Function

    Public Function cv_WordsFromHex(ByVal sHex As String) As Long()
        ' Converts string <sHex> with hex values into array of words (long ints)
        ' E.g. "fedcba9876543210" will be converted into {&HFEDCBA98, &H76543210}
        Const ncLEN As Integer = 8
        Dim i As Long
        Dim nWords As Long
        Dim aWords() As Long

        nWords = Len(sHex) \ ncLEN
        ReDim aWords(nWords - 1)
        For i = 0 To nWords - 1
            aWords(i) = Val("&H" & Mid(sHex, i * ncLEN + 1, ncLEN))
        Next
        Return aWords

    End Function

    Public Function cv_HexFromWords(ByRef aWords As Object) As String
        ' Converts array of words (Longs) into a hex string
        ' E.g. {&HFEDCBA98, &H76543210} will be converted to "FEDCBA9876543210"
        Const ncLEN As Integer = 8
        Dim i As Long
        Dim nWords As Long
        Dim sHex As New VB6.FixedLengthString(ncLEN)
        Dim iIndex As Long
        Dim s As String
        If Not IsArray(aWords) Then
            Exit Function
        End If

        nWords = UBound(aWords) - LBound(aWords) + 1
        s = New String(" ", nWords * ncLEN)
        iIndex = 0
        For i = 0 To nWords - 1
            sHex.Value = Hex(aWords(i))
            sHex.Value = New String("0", ncLEN - Len(sHex.Value)) & sHex.Value
            Mid(s, iIndex + 1, ncLEN) = sHex.Value
            iIndex = iIndex + ncLEN
        Next
        Return s
    End Function

    Public Function cv_HexFromBytes(ByRef aBytes() As Byte) As String
        ' Returns hex string from array of bytes
        ' E.g. aBytes() = {&HFE, &HDC, &H80} will return "FEDC80"
        Dim i As Long
        Dim iIndex As Long
        Dim s As String
        s = New String(" ", (UBound(aBytes) - LBound(aBytes) + 1) * 2)
        iIndex = 0
        For i = LBound(aBytes) To UBound(aBytes)
            Mid(s, iIndex + 1, 2) = HexFromByte(aBytes(i))
            iIndex = iIndex + 2
        Next
        Return s
    End Function

    Public Function cv_HexFromString(ByRef str_Renamed As String) As String
        ' Converts string <str> of ascii chars to string in hex format
        ' str may contain chars of any value between 0 and 255.
        ' E.g. "abc." will be converted to "6162632E"
        Dim byt As Byte
        Dim i As Long
        Dim n As Long
        Dim iIndex As Long
        Dim sHex As String

        n = Len(str_Renamed)
        sHex = New String(" ", n * 2)
        iIndex = 0
        For i = 1 To n
            byt = CByte(Asc(Mid(str_Renamed, i, 1)) And &HFFS)
            Mid(sHex, iIndex + 1, 2) = HexFromByte(byt)
            iIndex = iIndex + 2
        Next
        Return sHex

    End Function

    Public Function cv_StringFromHex(ByRef strHex As String) As String
        ' Converts string <strHex> in hex format to string of ascii chars
        ' with value between 0 and 255.
        ' E.g. "6162632E" will be converted to "abc."
        Dim i As Long
        Dim nBytes As Long
        Dim s As String
        nBytes = Len(strHex) \ 2
        s = New String(" ", nBytes)
        For i = 0 To nBytes - 1
            Mid(s, i + 1, 1) = Chr(Val("&H" & Mid(strHex, i * 2 + 1, 2)))
        Next
        Return s
    End Function

    Public Function cv_GetHexByte(ByVal sInputHex As String, ByRef iIndex As Long) As Byte
        ' Extracts iIndex'th byte from hex string (starting at 1)
        ' E.g. cv_GetHexByte("fecdba98", 3) will return &HBA
        Dim i As Long
        Dim b As Byte
        i = 2 * iIndex
        If i > Len(sInputHex) Or i <= 0 Then
            b = 0
        Else
            b = Val("&H" & Mid(sInputHex, i - 1, 2))
        End If
        Return b
    End Function

    Public Function RandHexByte() As String
        '   Returns a random byte as a 2-digit hex string
        Static stbInit As Boolean
        Dim s As String
        If Not stbInit Then
            Randomize()
            stbInit = True
        End If
        s = HexFromByte(CByte((Rnd() * 256) And &HFFS))
        Return s
    End Function

    Private Function HexFromByte(ByVal x As Object) As String
        x = x And &HFFS
        Dim s As String
        If x < 16 Then
            s = "0" & Hex(x)
        Else
            s = Hex(x)
        End If
        Return s
    End Function

    Public Function testBytesHex() As Byte()
        Dim aBytes() As Byte
        aBytes = cv_BytesFromHex("FEDC80")
        Return aBytes
    End Function

    Public Function testWordsHex() As Long()
        Dim aWords As Long()
        aWords = cv_WordsFromHex("FEDCBA9876543210")
        Return aWords
    End Function
End Module