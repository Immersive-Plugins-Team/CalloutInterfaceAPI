# CalloutInterfaceAPI [![Discord](https://img.shields.io/badge/discord-join-7289DA.svg)](https://discord.gg/AuJCUag)

CalloutInterfaceAPI is a library that makes interaction with the CalloutInterface plugin a breeze.

## Getting Started

Rather than import and reference CalloutInterface.dll directly, the CalloutInterfaceAPI library serves as a safe interface between your callout pack and CalloutInterface.  To get started, download the latest release and add CalloutInterfaceAPI.dll as a reference in your project.  Then make sure to bundle the .dll with your project similar to how you would include RageNativeUI.dll (in the root Grand Theft Auto V folder).

**You should NOT reference CalloutInterface directly.**

</br>

## Decorating your Callouts

One of the challenges of LSPDFR's callout system is that there is no separate description for callouts.  Some callout developers understandably add unique identifiers to their callout names which can cause them to look odd in CalloutInterface.  The solution is to extend LSPDFR's base CalloutInfoAttribute to add additional properties.  This allows CalloutInterface to silently add your callouts to the MDT while the player is unavailable for calls.  To support this functionality,
you need to modify the decorators of your callouts:

### Example

```cs
using CalloutInterfaceAPI;
using LSPD_First_Response.Mod.Callouts;

namespace ExampleNamespace
{
    [CalloutInterface("Your callout name", CalloutProbability.Medium, "A very useful description", "Code 2", "LSPD")]
    public class ExampleCallout : Callout
```

Note that the priority and agency are optional but you *must* provide a description.  The description is what will show up in the CalloutInterface log, so it should be concise and reflect the general nature of your callout.  It does not need to be unique.

![Description Example](https://i.imgur.com/AaYSI1B.png)

</br>

## Sending Messages

While your callout is active, you can send messages to the MDT.  You cannot use color codes, but newlines `\n` are supported.  CalloutInterface uses a timer to prevent spamming of messages so you should not send multiple messages in a row.

### Example
```cs
CalloutInterfaceAPI.Functions.SendMessage(this, "This is an example of a multi-line message.\nThis is the second line.\nPlease note, extremely long lines will be split up into chunks of no more than 60 characters.  The rest of this is just gibberish to demonstrate what it looks like.");
```
![Message Example](https://i.imgur.com/gYnx9KZ.png)

</br>

## Support

Please join my [discord server](https://discord.gg/AuJCUag) and open a ticket for CalloutInterface and mention that you are a callout pack developer.  I will add you to the CalloutInterfaceAPI channel.


## Special Thanks

Thanks to [Dylann24](https://github.com/Dylann24) and [Charlie686](https://github.com/Charlie-686) for being an early adopters and helping me test!
