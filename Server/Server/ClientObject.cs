using System;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class ClientObject
    {
        public TcpClient client;
        public ClientObject(TcpClient tcpClient)
        {
            client = tcpClient;
        }

        public void Process()
        {
            NetworkStream stream = null; // класс потоков для получения и отправки данных.
            
            try
            {

                while (true)
                {
                   
                    stream = client.GetStream();// возвращает объект NetworkStream
                    byte[] data = new byte[1024];// буфер для получаемых данных


                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    byte[] lenDataRec = new byte[4];
                    int lenDat = stream.Read(lenDataRec, 0, lenDataRec.Length);
                    string lol = Encoding.Unicode.GetString(lenDataRec,0, lenDat);
                    lenDat = Convert.ToInt32(lol);
                 
                    while (lenDat > 0)
                    {
                        if (data.Length > lenDat)
                        {
                            bytes = stream.Read(data, 0, lenDat);

                            builder.Append(Encoding.Unicode.GetString(data, 0, bytes)); //добавляет подстроку в конец объекта
                            lenDat = 0;
                        }
                        else
                        {
                            bytes = stream.Read(data, 0, data.Length);

                            builder.Append(Encoding.Unicode.GetString(data, 0, bytes)); //добавляет подстроку в конец объекта
                            lenDat -= data.Length;

                        }
                        
                    }
                    lenDataRec = null;
                    data = null;
                    

                    string message = builder.ToString(); //преобразование в строку
                    Console.WriteLine(message);

                    // отправляем обратно сообщение
                    byte[] lenData = new byte[4];
                    data = Encoding.Unicode.GetBytes(message); //перевод в байты.
                    lenData = Encoding.Unicode.GetBytes((data.Length).ToString());
                   
                    int mesLen = data.Length;
                    if (data.Length > 0)
                    {
                        stream.Write(lenData, 0, lenData.Length);
                        stream.Write(data, 0, data.Length);
                        
                    }
                    data = null;
                    lenData = null;
                }
            }
            /*проверка исключений*/
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close(); // закрывает поток
                if (client != null)
                    client.Close();
            }
        }
    }
}
