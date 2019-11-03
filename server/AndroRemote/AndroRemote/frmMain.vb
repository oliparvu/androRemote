Imports System
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Public Class frmMain
   
    Dim myIp As IPAddress

    Const DATA_LENGTH As Int32 = 10


    Const DATA As Byte = 254

    'commands section
    Const COMMAND As Byte = 253
    Const C_GET_HOST_RESOLUTION As Byte = 10


    Private Function GetLocalIPV4() As IPAddress

        Dim ips As IPAddress() = Dns.GetHostAddresses(Dns.GetHostName())

        For Each ip In ips
            'vezi daca e V4
            If ((Not IPAddress.IsLoopback(ip)) And (ip.AddressFamily = AddressFamily.InterNetwork) And (ip.ToString <> "255.255.255.255")) Then
                GetLocalIPV4 = ip
                Exit Function
            End If
        Next ip

        'mumu so baga NULL IP
        GetLocalIPV4 = IPAddress.None

    End Function


    Private Sub On_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim server As TcpListener
        server = Nothing
        Dim port As Int32 = 13000
        Dim localAddr As IPAddress = IPAddress.Parse("192.168.0.102")
        'Dim ips As IPAddress() = Dns.GetHostAddresses(Dns.GetHostName()).

        Dim aub_InBuffer(DATA_LENGTH) As Byte
        Dim aub_OutBuffer(DATA_LENGTH) As Byte


        ' Dim data As String = Nothing

        Dim client As TcpClient

        myIp = GetLocalIPV4()


        Try

            server = New TcpListener(myIp, port)
            server.Start()


            'blocking point...wait here for connection request
            client = server.AcceptTcpClient()
            Console.WriteLine("conectat")


            Dim stream As NetworkStream = client.GetStream()

            ' aub_OutBuffer(0) = DATA
            'stream.Write(aub_OutBuffer, 0, 1)

            While True


                Dim nrOfRecievedBytes As Int32

                'wait till 10bytes recieved (ToDo nu imi place asa)
                nrOfRecievedBytes = stream.Read(aub_InBuffer, 0, DATA_LENGTH)

                ''temp
                'nrOfRecievedBytes = stream.Read(aub_InBuffer, 0, 1)
                'Console.WriteLine("primit: {0}", aub_InBuffer(0))


                If nrOfRecievedBytes <> 0 Then

                    Select Case aub_InBuffer(0)

                        Case COMMAND
                            Select Case aub_InBuffer(1)

                                Case C_GET_HOST_RESOLUTION

                                    SendHostResolution(stream, aub_OutBuffer)

                            End Select
                        Case DATA
                            'do nothing
                        Case Else

                    End Select


                End If
            End While




            client.Close()

        Catch ex As SocketException
            Console.WriteLine("SocketException: {0}", e)
        Finally
            server.Stop()
        End Try

        
    End Sub

    Private Sub SendHostResolution(ByRef stream As NetworkStream, ByRef aub_OutBuffer As Byte())

        Dim aub_tempBuff(4) As Byte
        Dim screenWidth As Integer = Screen.PrimaryScreen.Bounds.Width
        Dim screenHeight As Integer = Screen.PrimaryScreen.Bounds.Height


        aub_tempBuff = BitConverter.GetBytes(screenWidth)

        aub_OutBuffer(0) = DATA
        aub_OutBuffer(1) = aub_tempBuff(0)
        aub_OutBuffer(2) = aub_tempBuff(1)
        aub_OutBuffer(3) = aub_tempBuff(2)
        aub_OutBuffer(4) = aub_tempBuff(3)

        aub_tempBuff = Nothing
        aub_tempBuff = BitConverter.GetBytes(screenHeight)

        aub_OutBuffer(5) = aub_tempBuff(0)
        aub_OutBuffer(6) = aub_tempBuff(1)
        aub_OutBuffer(7) = aub_tempBuff(2)
        aub_OutBuffer(8) = aub_tempBuff(3)

        stream.Write(aub_OutBuffer, 0, aub_OutBuffer.Length)

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        '  SendHostResolution()
    End Sub
End Class
