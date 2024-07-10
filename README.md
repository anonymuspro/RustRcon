# Welcome to RustRcon!

## Features:

> * _Server_
> > - Get ServerInfo.
> > - Stop/Restart server.
> > - Send console command.
> > - On chat/console message events.

> * _Player_
> > - Kick/Ban player.
> > - Get players ban list.
> > - Kill player.
> > - Get player info.

> * _Oxide_
> > - Get plugin list.
> > - Reload plugin.
> >  - Load/Unload plugin.

## Install:

### Self Build:

You should compile the library project and add a dependency to your project RustRcon.dll

### NuGet Gallery:

RustRcon is available on the NuGet Gallery, as still a release version:

* [NuGet Gallery: RustRcon_Client](https://www.nuget.org/packages/RustRcon_Client)

You can add RustRcon to your project with the NuGet Package Manager, by using the following command in the Package
Manager Console.

~~~ 
PM> Install-Package RustRcon_Client 
~~~

## Usage

#### Step 1:

Required namespace.

```cs
using RustRcon;
```

The RconClient class exists in the RustRcon namespace.

#### Step 2:

Creating a new instance of the RconClient class with the adress, port and password.

```cs
RconClient rconClient = new RconClient("localhost", 28016, "root");
```

#### Step 3:

Setting the events, example:

```cs
rconClient.OnChatMessage += (e) =>
{
    Console.WriteLine($"{e.Username}: {e.Message}");
    e.Dispose();
};

rconClient.OnConsoleMessage += (e) =>
{
    Console.WriteLine($"{e.Message}");
    e.Dispose();
};
```

#### Step 4:

Connecting to server.

```cs
await rconClient.ConnectAsync();
```

#### Step 5:

When sending a command to the server, commands take the necessary arguments in their constructor, after asynchronous waiting for the result the response will be in command.ServerResponse.Content (json) and if the command has a unique response, it will be in command.Result

```cs
var command = GetServerInfo.Create();
await rconClient.SendCommandAsync(command);

Console.WriteLine($"{command.Id} - {command.ServerResponse?.Id} : {command.Result?.Hostname}");
command.Dispose();
```

**Attention:** If the function does not have a specific return value, the `Result` will also contain `ServerResponse` <br>
And don't forget to call `Dispose` on commands and messages after use.
