Option Strict Off
Option Explicit On
Module basBlowfish


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


    ' basBlowfish: Bruce Schneier's Blowfish algorithm in VB
    ' Core routines.
    ' Version 5: January 2002. Speed improvements.
    ' Version 4: 12 May 2001. Fixed maxkeylen size from bits to bytes.
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

    ' Public Functions in this module:
    ' blf_EncipherBlock: Encrypts two words
    ' blf_DecipherBlock: Decrypts two words
    ' blf_Initialise: Initialise P & S arrays using key
    ' blf_KeyInit: Initialise using byte-array key
    ' blf_EncryptBytes: Encrypts an block of 8 bytes
    ' blf_DecryptBytes: Decrypts an block of 8 bytes
    '
    ' Superseded functions:
    ' blf_Key: Initialise using byte-array and its length
    ' blf_Enc: Encrypts an array of words
    ' blf_Dec: Decrypts an array of words

    Private Const ncROUNDS As Integer = 16
    Private Const ncMAXKEYLEN As Integer = 56
    ' Version 4: ncMAXKEYLEN was previously incorrectly set as 448
    ' (bits vs bytes)
    ' Thanks to Robert Garofalo for pointing this out.

    Private Function blf_F(ByRef x As Long) As Long
        Dim C, a, b, d As Byte
        Dim y As Long
        uwSplit(x, a, b, C, d)
        y = uw_WordAdd(blf_S(0, a), blf_S(1, b))
        y = y Xor blf_S(2, C)
        y = uw_WordAdd(y, blf_S(3, d))
        blf_F = y
    End Function

    Public Sub blf_EncipherBlock(ByRef xL As Long, ByRef xR As Long)
        Dim i As Long
        Dim temp As Long
        For i = 0 To ncROUNDS - 1
            xL = xL Xor blf_P(i)
            xR = blf_F(xL) Xor xR
            temp = xL
            xL = xR
            xR = temp
        Next
        temp = xL
        xL = xR
        xR = temp
        xR = xR Xor blf_P(ncROUNDS)
        xL = xL Xor blf_P(ncROUNDS + 1)
    End Sub

    Public Sub blf_DecipherBlock(ByRef xL As Long, ByRef xR As Long)
        Dim i As Long
        Dim temp As Long
        For i = ncROUNDS + 1 To 2 Step -1
            xL = xL Xor blf_P(i)
            xR = blf_F(xL) Xor xR
            temp = xL
            xL = xR
            xR = temp
        Next
        temp = xL
        xL = xR
        xR = temp
        xR = xR Xor blf_P(1)
        xL = xL Xor blf_P(0)
    End Sub

    Public Sub blf_Initialise(ByRef aKey() As Byte, ByRef nKeyBytes As Long)
        Dim j, i, K As Long
        Dim wDataL, wData, wDataR As Long
        blf_LoadArrays() ' Initialise P and S arrays
        j = 0
        For i = 0 To (ncROUNDS + 2 - 1)
            wData = &H0S
            For K = 0 To 3
                wData = uw_ShiftLeftBy8(wData) Or aKey(j)
                j = j + 1
                If j >= nKeyBytes Then j = 0
            Next K
            blf_P(i) = blf_P(i) Xor wData
        Next i
        wDataL = &H0S
        wDataR = &H0S
        For i = 0 To (ncROUNDS + 2 - 1) Step 2
            blf_EncipherBlock(wDataL, wDataR)
            blf_P(i) = wDataL
            blf_P(i + 1) = wDataR
        Next i
        For i = 0 To 3
            For j = 0 To 255 Step 2
                blf_EncipherBlock(wDataL, wDataR)
                blf_S(i, j) = wDataL
                blf_S(i, j + 1) = wDataR
            Next j
        Next i
    End Sub

    Public Function blf_Key(ByRef aKey() As Byte, ByRef nKeyLen As Long) As Boolean
        Dim blfKey As Boolean = False
        If nKeyLen < 0 Or nKeyLen > ncMAXKEYLEN Then
            Return blfKey
            Exit Function
        End If
        blf_Initialise(aKey, nKeyLen)
        blfKey = True
        Return blfKey
    End Function

    Public Function blf_KeyInit(ByRef aKey() As Byte) As Boolean
        ' Added Version 5: Replacement for blf_Key to avoid specifying keylen
        Dim nKeyLen As Long
        Dim blfKeyInit As Boolean = False
        nKeyLen = UBound(aKey) - LBound(aKey) + 1
        If nKeyLen < 0 Or nKeyLen > ncMAXKEYLEN Then
            Return blfKeyInit
            Exit Function
        End If
        blf_Initialise(aKey, nKeyLen)
        blfKeyInit = True
        Return blfKeyInit
    End Function

    Public Sub blf_EncryptBytes(ByRef aBytes() As Byte)
        ' aBytes() must be 8 bytes long
        ' Revised Version 5: January 2002. To use faster uwJoin and uwSplit fns.
        Dim wordL, wordR As Long
        ' Convert to 2 x words
        wordL = uwJoin(aBytes(0), aBytes(1), aBytes(2), aBytes(3))
        wordR = uwJoin(aBytes(4), aBytes(5), aBytes(6), aBytes(7))
        ' Encrypt it
        blf_EncipherBlock(wordL, wordR)
        ' Put back into bytes
        uwSplit(wordL, aBytes(0), aBytes(1), aBytes(2), aBytes(3))
        uwSplit(wordR, aBytes(4), aBytes(5), aBytes(6), aBytes(7))
    End Sub

    Public Sub blf_DecryptBytes(ByRef aBytes() As Byte)
        ' aBytes() must be 8 bytes long
        ' Revised Version 5:: January 2002. To use faster uwJoin and uwSplit fns.
        Dim wordL, wordR As Long
        ' Convert to 2 x words
        wordL = uwJoin(aBytes(0), aBytes(1), aBytes(2), aBytes(3))
        wordR = uwJoin(aBytes(4), aBytes(5), aBytes(6), aBytes(7))
        ' Decrypt it
        blf_DecipherBlock(wordL, wordR)
        ' Put back into bytes
        uwSplit(wordL, aBytes(0), aBytes(1), aBytes(2), aBytes(3))
        uwSplit(wordR, aBytes(4), aBytes(5), aBytes(6), aBytes(7))
    End Sub

    ' Version 5 note: These functions blf_Enc() and blf_Dec() are
    ' probably redundant now.
    ' See improved versions of blf_StringEnc and blf_StringDec

    Public Sub blf_Enc(ByRef awData() As Long, ByRef nWords As Long)
        ' Version 5: Changed Integer counters to Long
        Dim i As Long
        For i = 0 To nWords - 1 Step 2
            blf_EncipherBlock(awData(i), awData(i + 1))
        Next i
    End Sub

    Public Sub blf_Dec(ByRef awData() As Long, ByRef nWords As Long)
        ' Version 5: Changed Integer counters to Long
        Dim i As Long
        For i = 0 To nWords - 1 Step 2
            blf_DecipherBlock(awData(i), awData(i + 1))
        Next i
    End Sub
End Module