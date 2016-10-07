Imports System.Security.Cryptography
Imports BitManipulation.BitManipulator

'An implementation of ICryptoTransform for the Blowfish algorithm
Public Class BlowfishCrypto : Implements ICryptoTransform, ICompactCryptoTransform

    '_Random is used to determine random data for padding
    Private _Random As Random

    'The blowfish algorithm to use to perform the encryption/decryption
    Private _MyBlowfish As Blowfish

    'Indicates if this will be performing encryption or decryption
    Private _Encrypt As Boolean

    'Can only be constructed by a call to the BlowfishProvider class
    Friend Sub New(ByVal use As Blowfish, ByVal encrypt As Boolean)
        _Encrypt = encrypt
        _MyBlowfish = use
        _Random = New Random()
    End Sub

    'Implementation for Transform Block
    Private Function TransformBlock( _
       ByVal inputBuffer() As Byte, _
       ByVal inputOffset As Integer, _
       ByVal inputCount As Integer, _
       ByVal outputBuffer() As Byte, _
       ByVal outputOffset As Integer _
    ) As Integer Implements ICryptoTransform.TransformBlock, ICompactCryptoTransform.TransformBlock
        If _Encrypt Then
            Return EncryptBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset)
        Else
            Return DecryptBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset)
        End If
    End Function

    'Performs a block encryption
    Private Function EncryptBlock( _
       ByVal inputBuffer() As Byte, _
       ByVal inputOffset As Integer, _
       ByVal inputCount As Integer, _
       ByVal outputBuffer() As Byte, _
       ByVal outputOffset As Integer _
    ) As Integer

        Dim ret As Integer
        Dim i As Integer
        Dim val As Int64, res As Int64

        ret = 0

        For i = 0 To inputCount - 1 Step 8
            'Combine bypes into long
            val = PromoteToInt64(inputBuffer(inputOffset + i), 56)
            val += PromoteToInt64(inputBuffer(inputOffset + i + 1), 48)
            val += PromoteToInt64(inputBuffer(inputOffset + i + 2), 40)
            val += PromoteToInt64(inputBuffer(inputOffset + i + 3), 32)
            val += PromoteToInt64(inputBuffer(inputOffset + i + 4), 24)
            val += PromoteToInt64(inputBuffer(inputOffset + i + 5), 16)
            val += PromoteToInt64(inputBuffer(inputOffset + i + 6), 8)
            val += PromoteToInt64(inputBuffer(inputOffset + i + 7), 0)

            res = _MyBlowfish.Encrypt(val)

            'Store in output buffer
            outputBuffer(outputOffset + ret) = ExtractByte(res, 56)
            outputBuffer(outputOffset + ret + 1) = ExtractByte(res, 48)
            outputBuffer(outputOffset + ret + 2) = ExtractByte(res, 40)
            outputBuffer(outputOffset + ret + 3) = ExtractByte(res, 32)
            outputBuffer(outputOffset + ret + 4) = ExtractByte(res, 24)
            outputBuffer(outputOffset + ret + 5) = ExtractByte(res, 16)
            outputBuffer(outputOffset + ret + 6) = ExtractByte(res, 8)
            outputBuffer(outputOffset + ret + 7) = ExtractByte(res, 0)

            ret += 8
        Next

        Return ret

    End Function

    Private Function DecryptBlock( _
       ByVal inputBuffer() As Byte, _
       ByVal inputOffset As Integer, _
       ByVal inputCount As Integer, _
       ByVal outputBuffer() As Byte, _
       ByVal outputOffset As Integer _
    ) As Integer

        Dim ret As Integer
        Dim i As Integer
        Dim val As Int64, res As Int64
        Dim toKeep As Byte

        ret = 0

        For i = inputOffset To inputCount - 1 Step 8
            'Combine bypes into long
            val = PromoteToInt64(inputBuffer(inputOffset + i), 56)
            val += PromoteToInt64(inputBuffer(inputOffset + i + 1), 48)
            val += PromoteToInt64(inputBuffer(inputOffset + i + 2), 40)
            val += PromoteToInt64(inputBuffer(inputOffset + i + 3), 32)
            val += PromoteToInt64(inputBuffer(inputOffset + i + 4), 24)
            val += PromoteToInt64(inputBuffer(inputOffset + i + 5), 16)
            val += PromoteToInt64(inputBuffer(inputOffset + i + 6), 8)
            val += PromoteToInt64(inputBuffer(inputOffset + i + 7), 0)

            res = _MyBlowfish.Decrypt(val)

            'Store in output buffer
            outputBuffer(outputOffset + ret) = ExtractByte(res, 56)
            outputBuffer(outputOffset + ret + 1) = ExtractByte(res, 48)
            outputBuffer(outputOffset + ret + 2) = ExtractByte(res, 40)
            outputBuffer(outputOffset + ret + 3) = ExtractByte(res, 32)
            outputBuffer(outputOffset + ret + 4) = ExtractByte(res, 24)
            outputBuffer(outputOffset + ret + 5) = ExtractByte(res, 16)
            outputBuffer(outputOffset + ret + 6) = ExtractByte(res, 8)
            outputBuffer(outputOffset + ret + 7) = ExtractByte(res, 0)

            ret += 8
        Next

        Return ret
    End Function

    'Called to transform the final block
    ' Must always pad the final block
    Private Function TransformFinalBlock( _
       ByVal inputBuffer() As Byte, _
       ByVal inputOffset As Integer, _
       ByVal inputCount As Integer _
    ) As Byte() Implements ICompactCryptoTransform.TransformFinalBlock, ICryptoTransform.TransformFinalBlock

        Dim tmp(-1) As Byte
        Dim tmpOut() As Byte
        Dim i As Integer

        If inputCount Mod InputBlockSize = 0 Then
            'We have an exact size so use standard method
            ReDim tmp(OutputBlockSize - 1)
            TransformBlock(inputBuffer, inputOffset, inputCount, tmp, 0)

            If Not _Encrypt Then
                'Remove Padding
                If tmp(OutputBlockSize - 1) = InputBlockSize Then
                    'block fully padded
                    Return Nothing
                End If

                Dim res As Integer
                res = 1

                'Check for partial padding
                'Start with second byte as first byte would indicate a padding 
                'of the full block size...
                For i = OutputBlockSize - 1 To 0 Step -1
                    If tmp(i) = OutputBlockSize - i Then
                        ReDim tmpOut(i - 1)

                        'Copy data to tmpOut
                        Dim j As Integer
                        For j = 0 To i - 1
                            tmpOut(j) = tmp(j)
                        Next

                        Return tmpOut
                    ElseIf tmp(i) <> tmp(i - 1) Then
                        Throw New Exception("Error in Decryption")
                    End If
                Next
            Else
                'Encrypt fullblock + pad fullblock
                ReDim tmpOut(2 * OutputBlockSize - 1)

                For i = 0 To OutputBlockSize - 1
                    tmpOut(i) = tmp(i)
                Next

                'Pad with all random data and pad indicator
                For i = OutputBlockSize To (OutputBlockSize * 2) - 2
                    tmpOut(i) = _Random.Next() Mod 255
                Next
                tmpOut(i) = InputBlockSize

                TransformBlock(tmpOut, InputBlockSize, InputBlockSize, tmpOut, InputBlockSize)

                Return tmpOut
            End If
        Else
            If _Encrypt Then
                'Add padding...
                ReDim tmp(InputBlockSize - 1)

                If inputCount = 0 Then
                    'Pad with all random data and pad indicator
                    For i = 0 To InputBlockSize - 2
                        tmp(i) = _Random.Next() Mod 255
                    Next
                    tmp(i) = InputBlockSize
                Else
                    'Place data in array
                    For i = 0 To inputCount - 1
                        tmp(i) = inputBuffer(inputOffset + i)
                    Next

                    'Pad with PadSize indicator
                    For i = inputCount To 7
                        tmp(i) = (8 - inputCount)
                    Next
                End If

                ReDim tmpOut(OutputBlockSize - 1)
                TransformBlock(tmp, 0, InputBlockSize, tmpOut, 0)
                Return tmpOut
            Else
                'Cannot have error in size with decrypt...
                Throw New Exception("Decryption error...")
            End If
        End If

    End Function

    Private Sub Dispose() Implements IDisposable.Dispose
        _MyBlowfish = Nothing
    End Sub

    ReadOnly Property CanReuseTransform() As Boolean Implements ICryptoTransform.CanReuseTransform
        Get
            Return True
        End Get
    End Property

    ReadOnly Property CanTransformMultipleBlocks() As Boolean Implements ICryptoTransform.CanTransformMultipleBlocks
        Get
            Return True
        End Get
    End Property

    ReadOnly Property InputBlockSize() As Integer Implements ICryptoTransform.InputBlockSize, ICompactCryptoTransform.InputBlockSize
        Get
            Return 8
        End Get
    End Property

    ReadOnly Property OutputBlockSize() As Integer Implements ICryptoTransform.OutputBlockSize, ICompactCryptoTransform.OutputBlockSize
        Get
            Return 8
        End Get
    End Property
End Class
