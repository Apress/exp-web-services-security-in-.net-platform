Imports CryptoLib
Imports BlowfishLib
Imports BitManipulation.BitManipulator

Module Module1

    Sub Main()

        Dim testStr, eStr As String
        testStr = "Teststr" & Chr(200)

        eStr = StringCrypto.EncryptString(testStr)

        Console.WriteLine("*" & testStr & "*")
        Console.WriteLine("*" & eStr & "*")
        Console.WriteLine("*" & StringCrypto.DecryptString(eStr) & "*")

    End Sub

End Module
