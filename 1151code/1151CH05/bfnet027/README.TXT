
The project Blowfish.NET is fairly new and kind of as young 
as the new and modern .NET environment. Right now it consists 
of an ECB and a CBC implementation, an applied string encryption 
class, a wrapper for the SymmetricAlgorithmclass and some 
test applications. 

What is for sure today is the impressive speed, I'm measuring
around 38 MB/s on my P4/1.7 system.

For your comfort there is an easy to use application class
called BlowfishSimple, which is able to encrypt and decrypt
single strings on the fly, including key verification.

For comments, questions and suggestions please contact me
via e-mail. You can use this code in the current version free
without any charges in your applications, even in commercial
ones, as long as a copyright remark is mentioned somewhere
visible inside the software and/or the documentation.

Copyright (c) 2001-2003 Markus Hahn <markus_hahn@gmx.net>
