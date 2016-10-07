Imports System.IO

Public Interface ICompactCryptoTransform
    Function TransformBlock( _
           ByVal inputBuffer() As Byte, _
           ByVal inputOffset As Integer, _
           ByVal inputCount As Integer, _
           ByVal outputBuffer() As Byte, _
           ByVal outputOffset As Integer _
        ) As Integer

    Function TransformFinalBlock( _
       ByVal inputBuffer() As Byte, _
       ByVal inputOffset As Integer, _
       ByVal inputCount As Integer _
    ) As Byte()

    ReadOnly Property InputBlockSize() As Integer
    ReadOnly Property OutputBlockSize() As Integer


End Interface

Public Enum CompactCryptoMethod
    Read
    Write
End Enum

Public Class CompactCryptoStream : Inherits Stream

    Private _BaseStream As Stream
    Private _Transform As ICompactCryptoTransform
    Private _Method As CompactCryptoMethod

    Public Sub New(ByVal stream As Stream, ByVal transform As ICompactCryptoTransform, ByVal method As CompactCryptoMethod)
        _BaseStream = stream
        _Transform = transform
        _Method = method

        If _Method = CompactCryptoMethod.Read Then
            If Not _BaseStream.CanRead Then
                Throw New ApplicationException("Base stream does not allow read so Crypto method cannot be read")
            End If
        Else
            If Not _BaseStream.CanWrite Then
                Throw New ApplicationException("Base stream does not allow write so Crypto method cannot be write")
            End If
        End If
    End Sub

    Public Overrides ReadOnly Property CanRead() As Boolean
        Get
            Return _BaseStream.CanRead
        End Get
    End Property

    Public Overrides ReadOnly Property CanSeek() As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property CanWrite() As Boolean
        Get
            Return _BaseStream.CanWrite
        End Get
    End Property

    Public Overrides ReadOnly Property Length() As Long
        Get
            Throw New NotSupportedException()
        End Get
    End Property

    Public Overrides Property Position() As Long
        Get
            Throw New NotSupportedException()
        End Get
        Set(ByVal Value As Long)
            Throw New NotSupportedException()
        End Set
    End Property

    Public Sub Clear()
        _BaseStream = Nothing
        _Transform = Nothing
        _Method = Nothing
    End Sub

    Public Overrides Sub Close()
        _BaseStream.Close()
        MyBase.Close()
    End Sub

    Public Overrides Sub Flush()
        Return
    End Sub

    Public Sub FlushFinalBlock()
        Dim writeBuffer As Byte()
        If _DataBuffer Is Nothing Then
            writeBuffer = _Transform.TransformFinalBlock(Nothing, 0, 0)
        Else
            writeBuffer = _Transform.TransformFinalBlock(_DataBuffer, 0, _DataBuffer.Length)
        End If

        If Not writeBuffer Is Nothing Then
            _BaseStream.Write(writeBuffer, 0, writeBuffer.Length)
        End If
    End Sub

    Public Overrides Function Seek(ByVal offset As Long, ByVal origin As SeekOrigin) As Long
        Throw New NotSupportedException()
    End Function

    Public Overrides Sub SetLength(ByVal value As Long)
        Throw New NotSupportedException()
    End Sub

    Private _DataBuffer As Byte()

    Public Overrides Sub Write(ByVal buffer() As Byte, ByVal offset As Integer, ByVal count As Integer)
        'Buffer excess + (InputBlockSize)bytes of data...
        '8 byte excess always ensures that Transform Final block will be called
        'regardless of data input for encryption/decryption ...

        Dim toProcess As Integer
        Dim writeBuffer As Byte()

        Dim i As Integer

        If Not _DataBuffer Is Nothing Then
            'Copy _DataBuffer into write buffer
            ReDim writeBuffer(count + _DataBuffer.Length - 1)

            For i = 0 To _DataBuffer.Length - 1
                writeBuffer(i) = _DataBuffer(i)
            Next
        Else
            ReDim writeBuffer(count - 1)
        End If

        'Copy buffer into writeBuffer
        Dim j As Integer
        For j = i To writeBuffer.Length - 1
            writeBuffer(j) = buffer((j - i) + offset)
        Next

        'Determine how much to copy...
        Dim diff As Integer

        If (writeBuffer.Length Mod _Transform.InputBlockSize) = 0 Then
            toProcess = writeBuffer.Length - _Transform.InputBlockSize
            diff = _Transform.InputBlockSize
        Else
            diff = writeBuffer.Length Mod _Transform.InputBlockSize
        End If

        toProcess = writeBuffer.Length - diff

        'Save data which will not be written to the _DataBuffer
        ReDim _DataBuffer(diff - 1)
        For i = 0 To diff - 1
            _DataBuffer(i) = writeBuffer(toProcess + i)
        Next

        If toProcess > 0 Then
            'Setup output buffer
            Dim tmpBuff((toProcess / _Transform.InputBlockSize) * _Transform.OutputBlockSize - 1) As Byte
            Dim outLen As Integer

            outLen = _Transform.TransformBlock(writeBuffer, offset, toProcess, tmpBuff, 0)
            _BaseStream.Write(tmpBuff, 0, outLen)
        End If
    End Sub

    Public Overrides Function Read(ByVal buffer() As Byte, ByVal offset As Integer, ByVal count As Integer) As Integer
        Throw New NotSupportedException()
    End Function

End Class
