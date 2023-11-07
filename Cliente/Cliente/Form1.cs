using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Cliente
{
    public partial class Form1 : Form
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);

        public Form1()
        {
            InitializeComponent();
            contextMenuStrip1.Show();

        }
        private void atendermensaje()//esta funcion recibe los mensajes del servidor y manda los datos a la funcion necesaria
        {
            while (true)
            {
                byte[] respuesta = new byte[1024];
                socket.Receive(respuesta);
                string[] trozos = Encoding.ASCII.GetString(respuesta).Split("/");
                int codigo = Convert.ToInt32(trozos[0]);

                switch (codigo)
                {

                    case 1: // recibe la confirmacion del inicio de sesion
                        int a = Convert.ToInt32(trozos[1]);
                        if (a == 0)
                        {
                            MessageBox.Show("has iniciado sesion con exito");

                        }
                        else
                        {
                            MessageBox.Show("No has podido iniciar sesion");
                        }
                        break;
                    case 2: //Registrar
                        if (trozos[1] != "error")
                        {
                            MessageBox.Show("has podido registrate");
                        }
                        else
                        {
                            MessageBox.Show("No se ha  podido registrasrse");
                        }

                        break;
                    case 3: //lista
                        int b = Convert.ToInt32(trozos[1]);
                        dataGridView1.Rows.Add(b);
                        dataGridView1.Rows[0].Cells[0].Value = "Lista de conectados";
                        for (int i = 3; i < b; i++)
                        {
                            dataGridView1.Rows[i].Cells[0].Value = trozos[i];
                        }
                        break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e) // inicia sesion 
        {

            try
            {

                string usuario = textBox1.Text;
                string contraseña = textBox2.Text;
                string mensaje = ("1/" + usuario + "/" + contraseña); // envia al servidor el inicio de sesion 
                socket.Send(Encoding.ASCII.GetBytes(mensaje));



            }
            catch (SocketException)
            {
                label1.Text = "Imposible iniciar sesion ";
                MessageBox.Show("No has podido iniciar sesion ");

            }
        }

        private void button2_Click(object sender, EventArgs e) //Cerrar sesion
        {
            try
            {
                string usuario = textBox1.Text;
                string sesion = ("3/" + usuario); //cierra sesion del usuario
                socket.Send(Encoding.ASCII.GetBytes(sesion));
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                MessageBox.Show("desconectado del servidor");
                label1.Text = "Estas desconectado del servidor";



            }
            catch (SocketException)
            {

                label1.Text = "Imposible desconectarse del servidor";
            }

        }

        private void timer1_Tick(object sender, EventArgs e) //pide la lista de conectados cada 3 segundos
        {
            string mensaje = "3/";
            socket.Send(Encoding.ASCII.GetBytes(mensaje));

        }

        private void comoFuncionaElProgramaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("El programa se conecta al servidor directamente cuando lo abres(si no va dele al boton conectarse)  y para iniciar sesion simplemente tienes que rellenar los textbox");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                socket.Connect(remoteEP);
                label1.Text = "ya estas conectado al servidor";

            }
            catch (Exception)
            {
                label1.Text = "No te has podido conectar al server";

            }

        }

        private void button4_Click(object sender, EventArgs e) //registrarse
        {

            try
            {

                string usuario = textBox1.Text;
                string contraseña = textBox2.Text;
                string mensaje = ("2/" + usuario + "/" + contraseña); // envia al servidor el registro 
                socket.Send(Encoding.ASCII.GetBytes(mensaje));



            }
            catch (SocketException)
            {
                MessageBox.Show("No has podido crear un usuario ");

            }
        }
    }
}

