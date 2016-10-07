Option Strict Off
Option Explicit On
Module basBlowfishFns


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


	' basBlowfishFns: Wrapper functions to call Blowfish algorithms
	
	' Version 5. January 2002. Completely revised blf_String fns.
	' Added blf_StringRaw() fn and PadString and UnpadString fns.
	' File functions moved to basBlowfishFileFns module.
	' Many thanks to Robert Garofalo and Doug J Ward for suggestions
	' and advice greatly appreciated.
	' Version 4. 12 May 2001. Improvements as noted.
	' Version 2. Published 16 November 2000
	' First published October 2000.
	'************************* COPYRIGHT NOTICE*************************
	' This code was originally written in Visual Basic by David Ireland
	' and is copyright (c) 2000-2 D.I. Management Services Pty Limited,
	' all rights reserved.
	
	' You are free to use this code as part of your own applications
	' provided you keep this copyright notice intact and acknowledge
	' its authorship with the words:
	
	'   "Contains cryptography software by David Ireland of
	'   DI Management Services Pty Ltd <www.di-mgt.com.au>."
	
	' If you use it as part of a web site, please include a link
	' to our site in the form
	' <A HREF="http://www.di-mgt.com.au/crypto.html">Cryptography
	' Software Code</a>
	
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
	
	' The functions in this module are:
	' blf_StringEnc(strData): Enciphers string strData with current key
	' blf_StringDec(strData): Deciphers string strData with current key
	' blf_StringRaw(strData, bEncrypt): En/Deciphers strData without padding
	' PadString(strData): Pads data string to next multiple of 8 bytes
	' UnpadString(strData): Removes padding after decryption
	
	' To set current key, call blf_KeyInit(aKey())
	'   where aKey() is the key as an array of Bytes

    Public Function blf_ByteCrypt(ByRef bytData As Byte(), ByVal blnEncrypt As Boolean) As Byte()
        Dim strIn As String
        Dim nLen As Long
        Dim sPad As String
        Dim nPad As Long
        Dim nBlocks As Long
        Dim h, i As Long
        Dim j, k As Long
        Dim aBytes() As Byte
        Dim sBlock(7) As Byte
        Dim iIndex As Long

        'If ENCRYPTING, Pad data string to multiple of 8 bytes
        nLen = bytData.Length
        If blnEncrypt = True Then
            nPad = ((nLen \ 8) + 1) * 8 - nLen
            If nLen Mod 8 <> 0 Then
                ReDim Preserve bytData(bytData.Length + nPad - 1)
                For h = 1 To nPad
                    bytData(nLen + h - 1) = nPad
                Next
                nLen = bytData.Length
            End If
        End If
        nBlocks = nLen \ 8

        Dim strOut(nLen - 1) As Byte
        ' Work through string in blocks of 8 bytes
        iIndex = 0
        For i = 1 To nBlocks
            For j = 0 To 7
                sBlock(j) = bytData(iIndex + j)
            Next
            If blnEncrypt = True Then
                blf_EncryptBytes(sBlock)
            Else
                blf_DecryptBytes(sBlock)
            End If

            For k = 0 To 7
                strOut(k + iIndex) = sBlock(k)
            Next

            iIndex = iIndex + 8
        Next
        'If DECRYPTING, remove padding
        If blnEncrypt = False Then
            nPad = strOut(strOut.Length - 1)
            If nPad > 7 Then nPad = 0
            ReDim Preserve strOut(strOut.Length - nPad - 1)
        End If

        Return strOut
    End Function

    Public Function blf_StringEnc(ByRef strData As String) As String
        ' Encrypts plaintext strData after adding RFC 2630 padding
        ' Returns encrypted string.
        ' Requires key and boxes to be already set up.
        ' Version 5. Completely revised.
        ' The speed improvement here is due to Robert Garofalo.
        Dim strIn As String
        Dim strOut As String
        Dim nLen As Long
        Dim sPad As String
        Dim nPad As Long
        Dim nBlocks As Long
        Dim i As Long
        Dim j As Long
        Dim aBytes() As Byte
        Dim sBlock As New VB6.FixedLengthString(8)
        Dim iIndex As Long

        ' Pad data string to multiple of 8 bytes
        nLen = Len(strData)
        nPad = ((nLen \ 8) + 1) * 8 - nLen
        sPad = New String(Chr(nPad), nPad) ' Pad with # of pads (1-8)
        strIn = strData & sPad
        ' Calc number of 8-byte blocks
        nLen = Len(strIn)
        nBlocks = nLen \ 8
        ' Allocate output string here so we can use Mid$ below
        strOut = New String(" ", nLen)

        ' Work through string in blocks of 8 bytes
        iIndex = 0
        For i = 1 To nBlocks
            sBlock.Value = Mid(strIn, iIndex + 1, 8)
            ' Convert to bytes
            aBytes = System.Text.Encoding.Default.GetBytes(sBlock.Value)
            ' Encrypt the Block
            blf_EncryptBytes(aBytes)
            ' Convert back to a string
            sBlock.Value = System.Text.Encoding.Default.GetString(aBytes)
            ' Copy to output string
            Mid(strOut, iIndex + 1, 8) = sBlock.Value
            iIndex = iIndex + 8
        Next

        Return strOut
    End Function

    Public Function blf_StringDec(ByRef strData As String) As String
        ' Decrypts ciphertext strData and removes RFC 2630 padding
        ' Returns decrypted string.
        ' Requires key and boxes to be already set up.
        ' Version 5. Completely revised.
        ' The speed improvement here is due to Robert Garofalo.
        Dim strIn As String
        Dim strOut As String
        Dim nLen As Long
        Dim sPad As String
        Dim nPad As Long
        Dim nBlocks As Long
        Dim i As Long
        Dim j As Long
        Dim aBytes() As Byte
        Dim sBlock As New VB6.FixedLengthString(8)
        Dim iIndex As Long

        strIn = strData
        ' Calc number of 8-byte blocks
        nLen = Len(strIn)
        nBlocks = nLen \ 8
        ' Allocate output string here so we can use Mid$ below
        strOut = New String(" ", nLen)

        ' Work through string in blocks of 8 bytes
        iIndex = 0
        For i = 1 To nBlocks
            sBlock.Value = Mid(strIn, iIndex + 1, 8)
            ' Convert to bytes
            aBytes = System.Text.Encoding.Default.GetBytes(sBlock.Value)
            ' Encrypt the block
            blf_DecryptBytes(aBytes)
            ' Convert back to a string
            sBlock.Value = System.Text.Encoding.Default.GetString(aBytes)
            ' Copy to output string
            Mid(strOut, iIndex + 1, 8) = sBlock.Value
            iIndex = iIndex + 8
        Next

        ' Strip padding, if valid
        nPad = Asc(Right(strOut, 1))
        If nPad > 8 Then nPad = 0
        strOut = Left(strOut, nLen - nPad)

        Return strOut
    End Function

    Public Function blf_StringRaw(ByRef strData As String, ByRef bEncrypt As Boolean) As String
        ' New function added version 5.
        ' Encrypts or decrypts strData without padding according to current key.
        ' Similar to blf_StringEnc and blf_StringDec, but does not add padding
        ' and ignores trailing odd bytes.
        Dim strIn As String
        Dim strOut As String
        Dim nLen As Long
        Dim nBlocks As Long
        Dim i As Long
        Dim j As Long
        Dim aBytes() As Byte
        Dim sBlock As New VB6.FixedLengthString(8)
        Dim iIndex As Long

        ' Calc number of 8-byte blocks (ignore odd trailing bytes)
        strIn = strData
        nLen = Len(strIn)
        nBlocks = nLen \ 8
        ' Allocate output string here so we can use Mid$ below
        strOut = New String(" ", nLen)

        ' Work through string in blocks of 8 bytes
        iIndex = 0
        For i = 1 To nBlocks
            sBlock.Value = Mid(strIn, iIndex + 1, 8)
            ' Convert to bytes
            aBytes = System.Text.Encoding.Default.GetBytes(sBlock.Value)
            ' En/Decrypt the block according to flag
            If bEncrypt Then
                blf_EncryptBytes(aBytes)
            Else
                blf_DecryptBytes(aBytes)
            End If
            ' Convert back to a string
            sBlock.Value = System.Text.Encoding.Default.GetString(aBytes)
            ' Copy to output string
            Mid(strOut, iIndex + 1, 8) = sBlock.Value
            iIndex = iIndex + 8
        Next

        Return strOut
    End Function

    ' PadString() and UnpadString() fns added in version 5.

    Public Function PadString(ByRef strData As String) As String
        ' Pad data string to next multiple of 8 bytes as per RFC 2630
        Dim nLen As Long
        Dim sPad As String
        Dim nPad As Long
        nLen = Len(strData)
        nPad = ((nLen \ 8) + 1) * 8 - nLen
        sPad = New String(Chr(nPad), nPad) ' Pad with # of pads (1-8)
        PadString = strData & sPad
    End Function

    Public Function UnpadString(ByRef strData As String) As String
        ' Strip RFC 2630-style padding
        Dim nLen As Long
        Dim nPad As Long
        nLen = Len(strData)
        If nLen = 0 Then Exit Function
        ' Get # of padding bytes from last char
        nPad = Asc(Right(strData, 1))
        If nPad > 8 Then nPad = 0 ' In case invalid
        UnpadString = Left(strData, nLen - nPad)
    End Function
End Module