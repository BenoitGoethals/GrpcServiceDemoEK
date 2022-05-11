# Performances

We can measure the performances of our grpc by using different tools, one of which is `ghz`

You can download it from [here](https://github.com/bojand/ghz/releases)

To test the IPC Demo:

```powershell
ghz --insecure --call greet.Greeter.SayHello -d "{\"name\":\"Simona\"}" "unix:C:\\Users\\SimonaC\\AppData\\Local\\Temp\\socket.tmp" -c 10 -n 10000 --rps 200
```

```powershell
ghz --insecure --call greet.Greeter.ClientStreaming -d "[{\"name\":\"Simona\"},{\"name\":\"Mario\"}]" "unix:C:\\Users\\SimonaC\\AppData\\Local\\Temp\\socket.tmp" -c 10 -n 10000 --rps 200
```

To Test the unary call of the simple greeter demo:
```powershell
ghz--insecure--call greet.Greeter.SayHello -d "{\"name\":\"Simona\"}" localhost:5164 -c 10 -n 10000 --rps 200
```

