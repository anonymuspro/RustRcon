# Welcome to RustRcon!
## Features:
>* _Server_
>   - Get ServerInfo.
>   - Stop/Restart server.
>   - Send console command.
>   - On chat/console message events.

>* _Player_
>   - Kick/Ban player.
>   - Get players ban list.
>   - Kill player.
>   - Get player info.

>* _Oxide_
>   - Get plugin list.
>   - Reload plugin.
>   - Load/Unload plugin.

## Install:
### Self Build: 
You should compile the library project and add a dependency to your project RustRcon.dll
### NuGet Gallery:
RustRcon is available on the NuGet Gallery, as still a release version:
* [NuGet Gallery: RustRcon_Client](https://www.nuget.org/packages/RustRcon_Client)

You can add RustRcon to your project with the NuGet Package Manager, by using the following command in the Package Manager Console.
~~~ 
PM> Install-Package RustRcon_Client -Version 1.0.0 
~~~

## Usage
#### Step 1:
Required namespace.
~~~C#
using RustRcon;
~~~
The RconClient class exists in the RustRcon namespace.

#### Step 2: 
Creating a new instance of the RconClient class with the adress, port and password.
~~~C#
RconClient rconClient = new RconClient("localhost", 28016, "root");
~~~


#### Step 3:
Setting the events, example:
~~~C#
rconClient.OnChatMessage += (e) =>
{
    Console.WriteLine($"{e.Username}: {e.Message}");
};
~~~

#### Step 4:
Connecting to server.
~~~C#
rconClient.Connect();
~~~

#### Step 5:
Sending command to server, the commands accept in their constructor a reference to the callback action with the result of the request:
~~~C#
rconClient.SendCommand(new GetServerInfo((x) =>
{
    Console.WriteLine($"Connected to: {x.Hostname}");
}));
~~~
