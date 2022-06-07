# C# loader that copies an encoded shellcode in memory and decodes in after being in memory already

Uses p/invoke

`ProgramAmsiEtwPatch` also patches `AmsiScanBuffer` and `EtwEventWrite`

Tested with Meterpreter staged rev HTTPS payload (`encode_shellcode.cs` or py version is the code I used to encode the raw one)

ProgramAmsiEtwPatch.cs against SentinelOne (used Babel .net obfuscator - free version - twice on the resulting exe)

![Windowz](https://github.com/clod81/shellcode_runner_copy_encoded_decode_after/blob/main/3.png?raw=true "Windowz")

![Meterpreter](https://github.com/clod81/shellcode_runner_copy_encoded_decode_after/blob/main/4.png?raw=true "Meterpreter")

Program.cs against Defender

![Windowz](https://github.com/clod81/shellcode_runner_copy_encoded_decode_after/blob/main/1.png?raw=true "Windowz")

![Meterpreter](https://github.com/clod81/shellcode_runner_copy_encoded_decode_after/blob/main/2.png?raw=true "Meterpreter")
