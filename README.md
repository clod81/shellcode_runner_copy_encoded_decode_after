# C# loader that copies an encoded shellcode in memory and decodes in after being in memory already

Uses p/invoke

Tested with Meterpreter staged rev HTTPS payload (`encode_shellcode.cs` is the code I used to encode the raw one)

![Windowz](https://github.com/clod81/shellcode_runner_copy_encoded_decode_after/blob/main/1.png?raw=true "Windowz")

![Meterpreter](https://github.com/clod81/shellcode_runner_copy_encoded_decode_after/blob/main/2.png?raw=true "Meterpreter")
