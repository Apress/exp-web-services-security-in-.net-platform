Imports System.IO
Imports System.Security
Imports System.Text
Imports BlowfishLib

Public Class StringCrypto

    Private Shared clientBlowfish As Blowfish

    Shared Sub New()
        clientBlowfish = New Blowfish(New Integer() {-872134, 1274921794, -278998273, 1276438614})
    End Sub

    Public Shared Function EncryptString(ByVal AString As String) As String

        If AString = String.Empty Then
            Return AString
        Else
            Dim encryptedData() As Byte
            Dim dataStream As MemoryStream

            Dim encryptor As ICompactCryptoTransform
            encryptor = clientBlowfish.CreateEncryptor()

            Try
                dataStream = New MemoryStream()

                Dim encryptedStream As CompactCryptoStream
                Try
                    'Create the encrypted stream
                    encryptedStream = New CompactCryptoStream(dataStream, encryptor, CompactCryptoMethod.Write)

                    'Dim theWriter As StreamWriter
                    Try
                        'Write the string to memory via the encryption algorithm
                        'theWriter = New StreamWriter(encryptedStream, System.Text.Encoding.UTF8)
                        'Write the string to the memory stream
                        'theWriter.Write(AString)

                        'End the writing
                        'theWriter.Flush()
                        Dim tmpData(AString.Length - 1) As Byte
                        Dim i As Integer

                        For i = 0 To AString.Length - 1
                            tmpData(i) = Asc(AString.Chars(i))
                        Next

                        encryptedStream.Write(tmpData, 0, AString.Length)
                        encryptedStream.FlushFinalBlock()

                        'Position back at start
                        dataStream.Position = 0

                        'Create area for data
                        ReDim encryptedData(dataStream.Length - 1)

                        'Read data from memory
                        dataStream.Read(encryptedData, 0, dataStream.Length)

                        'Convert to String
                        Return Convert.ToBase64String(encryptedData)
                    Finally
                        'theWriter.Close()
                    End Try
                Finally
                    encryptedStream.Close()
                End Try
            Finally
                dataStream.Close()
            End Try
        End If
    End Function

    Public Shared Function DecryptString(ByVal AString As String) As String
        If AString = String.Empty Then
            Return AString
        Else
            Dim encryptedData() As Byte
            Dim dData() As Char
            Dim dataStream As MemoryStream
            'Dim myReader As StreamReader
            Dim encryptedStream As CompactCryptoStream
            Dim strLen As Integer

            'Get the byte data
            encryptedData = Convert.FromBase64String(AString)

            Try
                dataStream = New MemoryStream()
                Try
                    'Create decryptor and stream
                    Dim decryptor As ICompactCryptoTransform
                    decryptor = clientBlowfish.CreateDecryptor()
                    encryptedStream = New CompactCryptoStream(dataStream, decryptor, CompactCryptoMethod.Write)


                    'Write the decrypted data to the memory stream
                    encryptedStream.Write(encryptedData, 0, encryptedData.Length)
                    encryptedStream.FlushFinalBlock()

                    'Position back at start
                    dataStream.Position = 0

                    'Create reader to read stream
                    'myReader = New StreamReader(dataStream, System.Text.Encoding.UTF8)

                    'Read the string from the stream and return
                    'Return myReader.ReadToEnd()

                    Dim ret As StringBuilder
                    Dim eData() As Byte
                    Dim i As Integer

                    eData = dataStream.ToArray()
                    ret = New StringBuilder()

                    For i = 0 To eData.Length - 1
                        ret.Append(Chr(eData(i)))
                    Next

                    Return ret.ToString()
                Finally
                    encryptedStream.Close()
                End Try
            Finally
                dataStream.Close()
            End Try
        End If
    End Function

End Class
