# Sleipnir
Released as open source by NCC Group Plc - http://www.nccgroup.com/

Developed by Dave Spencer, david [dot] spencer [at] nccgroup [dot] com

http://www.github.com/nccgroup/Sleipnir


Released under AGPL see LICENSE for more information
## Description
Sleipnir is a small executable designed for quick upload via the send keys method within Loki, hence the lack of an icon. Sleipnir was developed to provide two-way file transfers at a much quicker pace than the send keys method could provide by using virtual channels.
A virtual channel needs to be created on the server side and then connected to from the client side, any data passed through these channels is done so via the RDP connection. The .NET function AxMsRdpClient7NotSafeForScripting.SendOnVirtualChannel can only send string data of a limited length, for this reason the files are carved in to 1k chunks and base64 encoded on one side and then then base64 decoded into a stream on the other side.

Sleipnir is designed to be used with Loki (https://github.com/nccgroup/loki) and Fenrir (https://github.com/nccgroup/Fenrir)

## Usage
Sleipnir consists of a form with a progress bar and two buttons:

1)	Receive File – sets Sleipnir to listen for events on the virtual channel Loki1

2)	Send File – allows the user to select a file on the RDP desktop to send back to the client.

To send a file from the local host to the RDP server the user must click the Receive File button on Sleipnir, and then click “Sleipnir” -> “Send File On Virtual Channel” within the Loki interface, the user is then prompted to select a file. 
Sleipnir then reads the file in 1k at a time, base64 encodes it and sends it to the virtual channel. An event handler within Sleipnir detects the write, decodes it and writes it to a file. The file is created in the same directory as Sleipnir is located and retains its original file name.
To send a file from the RDP server back to the client simply click the Send File button select a file and then wait for the transfer to end.
With some of the larger files there is a little delay in the update of the progress bar, and it may appear to have crashed when in fact it is working. While it should be able to transfer files of any size I have only tested with files of up to 250mb, which it handled perfectly well (although the progress bar did not update, you just have to wait for the message box saying it transferred ok). Throughput averages about 800kb/s.
