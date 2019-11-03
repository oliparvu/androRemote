Imports System
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text


Public Class Form1


    Const PORT As Int32 = 3128
    Dim myIp As IPAddress
    Dim data(128) As Byte
    Dim stream As NetworkStream
    Const DATAs As Byte = 255

    'commands section
    Const COMMAND As Byte = 254
    Const C_GET_HOST_RESOLUTION As Byte = 10


    Private Function GetLocalIPV4() As IPAddress
        Dim ips As IPAddress() = Dns.GetHostAddresses(Dns.GetHostName())

        For Each ip In ips
            'test if IPV4
            If ((Not IPAddress.IsLoopback(ip)) And (ip.AddressFamily = AddressFamily.InterNetwork) And (ip.ToString <> "255.255.255.255")) Then
                GetLocalIPV4 = ip
                Exit Function
            End If
        Next ip

        'no suitable IPV4 found so return a NULL IP adress
        GetLocalIPV4 = IPAddress.None

    End Function


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim Message As String = "plm"
        Dim port As Int32 = 13000
      

        myIp = GetLocalIPV4()

            Try

            Dim client As New TcpClient(myIp.ToString, port)

            stream = client.GetStream()


            data(0) = COMMAND
            data(1) = C_GET_HOST_RESOLUTION
              



           

            'Console.WriteLine("Sent: {0}", Message)

            '' Receive the TcpServer.response.
            '' Buffer to store the response bytes.
            'data = New [Byte](256) {}

            '' String to store the response ASCII representation.
            'Dim responseData As [String] = [String].Empty

            '' Read the first batch of the TcpServer response bytes.
            'Dim bytes As Int32 = stream.Read(data, 0, data.Length)
            'responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes)
            'Console.WriteLine("Received: {0}", responseData)

            '' Close everything.
            'stream.Close()
            'client.Close()
            Catch ex As ArgumentNullException
                Console.WriteLine("ArgumentNullException: {0}", e)
            Catch ex As SocketException
                Console.WriteLine("SocketException: {0}", e)
            End Try

            Console.WriteLine(ControlChars.Cr + " Press Enter to continue...")
        ' Console.Read()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click


        stream.Write(data, 0, 2)
        Do
            'Wait for client to send request (command or data)
        Loop Until Stream.DataAvailable

        Dim bytes As Int32 = Stream.Read(DATA, 0, DATA.Length)
        Dim aub_tempBuff(4) As Byte
        Dim width, height

        aub_tempBuff(0) = data(1)
        aub_tempBuff(1) = data(2)
        aub_tempBuff(2) = data(3)
        aub_tempBuff(3) = data(4)
        width = BitConverter.ToInt32(data, 1)
        height = BitConverter.ToInt32(data, 5)

        Dim responseData As [String] = [String].Empty

        responseData = System.Text.Encoding.ASCII.GetString(DATA, 0, bytes)
        Console.WriteLine("Received: {0} {1}", width.ToString, height.ToString)
    End Sub
End Class
