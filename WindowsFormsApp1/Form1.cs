using Model;
using System;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Windows.Forms;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public List<Usuario> listaUsuarios;

        public Form1()
        {
            listaUsuarios = new List<Usuario>();
            InitializeComponent();
        }

        private void CreatePersonAsync(LogMaquina person)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://127.0.0.1:5000/");
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // Transforma o objeto em json
                string json = JsonConvert.SerializeObject(person);

                // Envia o json para a API e verifica se obteve sucesso
                HttpResponseMessage response =  client.PostAsync("api", new StringContent(json, Encoding.UTF8, "application/json")).Result;
                if (response.IsSuccessStatusCode)
                {
                    textBox1.AppendText("OK Http - Codigo: " + person.Codigo + " Mensagem: " + person.Mensagem +  "\n");
                }
                else
                {
                    textBox1.AppendText("Erro Http - Codigo: " + person.Codigo + " Mensagem: " + person.Mensagem + "\n");
                }
            }
            catch (Exception ex)
            {
                textBox1.AppendText("Erro Program " + ex + "\n");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            edId.Text = "0";
            edNome.Text = "";
            edBairro.Text = "";
            edCidade.Text = "";
            edEmail.Text = "";
        }

        private void listaUsu_Click(object sender, EventArgs e)
        {
            atualizaListaUsuario();
        }

        public void atualizaListaUsuario()
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://127.0.0.1:5555/");
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // Envia o json para a API e verifica se obteve sucesso
                var task = client.GetAsync("/listaUsuarios").ContinueWith((taskwithresponse) =>
                {
                    var response = taskwithresponse.Result;
                    var jsonString = response.Content.ReadAsStringAsync();
                    jsonString.Wait();
                    listaUsuarios = JsonConvert.DeserializeObject<List<Usuario>>(jsonString.Result);
                });
                task.Wait();
                AtualizaGrade();
            }
            catch (Exception ex)
            {
                textBox1.AppendText("Erro Program " + ex + "\n");
            }
        }

        public void AtualizaGrade()
        {
            this.dataGridView1.DataSource = listaUsuarios;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //inserir || atualizar
            if (edId.Text == "0")
            {
                Usuario usuario = new Usuario();
                usuario.nome = edNome.Text;
                usuario.bairro = edBairro.Text;
                usuario.cidade = edCidade.Text;
                usuario.email = edEmail.Text;
                try
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri("http://127.0.0.1:5555/");
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    // Transforma o objeto em json
                    string json = JsonConvert.SerializeObject(usuario);

                    // Envia o json para a API e verifica se obteve sucesso
                    HttpResponseMessage response = client.PutAsync("cadastro", new StringContent(json, Encoding.UTF8, "application/json")).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        textBox1.AppendText("OK Http - Codigo: " + response.StatusCode + " Mensagem: " + response.RequestMessage + "\n");
                    }
                    else
                    {
                        textBox1.AppendText("Erro Http - Codigo: " + response.StatusCode + " Mensagem: " + response.RequestMessage + "\n");
                    }
                }
                catch (Exception ex)
                {
                    textBox1.AppendText("Erro Program " + ex + "\n");
                }
            }
            else
            {
                //atualizar
                Usuario usuario = new Usuario();
                usuario.nome = edNome.Text;
                usuario.bairro = edBairro.Text;
                usuario.cidade = edCidade.Text;
                usuario.email = edEmail.Text;
                try
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri("http://127.0.0.1:5555/");
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    // Transforma o objeto em json
                    string json = JsonConvert.SerializeObject(usuario);

                    // Envia o json para a API e verifica se obteve sucesso
                    HttpResponseMessage response = client.PostAsync("atualizar/" + edId.Text, new StringContent(json, Encoding.UTF8, "application/json")).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        textBox1.AppendText("OK Http - Codigo: " + response.StatusCode + " Mensagem: " + response.RequestMessage + "\n");
                    }
                    else
                    {
                        textBox1.AppendText("Erro Http - Codigo: " + response.StatusCode + " Mensagem: " + response.RequestMessage + "\n");
                    }
                }
                catch (Exception ex)
                {
                    textBox1.AppendText("Erro Program " + ex + "\n");
                }

            }
            atualizaListaUsuario();
        }
        
        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            edId.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            edNome.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            edBairro.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            edCidade.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            edEmail.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Excluir
            try
            {
                if (edId.Text != "0")
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri("http://127.0.0.1:5555/");
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    // Envia o json para a API e verifica se obteve sucesso
                    HttpResponseMessage response = client.DeleteAsync("excluir/" + edId.Text).Result;
                    // response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                        textBox1.AppendText("OK Http - Codigo: " + response.StatusCode + " Mensagem: " + response.RequestMessage + "\n");
                    }
                    else
                    {
                        textBox1.AppendText("Erro Http - Codigo: " + response.StatusCode + " Mensagem: " + response.RequestMessage + "\n");
                    }
                }
                atualizaListaUsuario();
            }
            catch (Exception ex)
            {
                textBox1.AppendText("Erro Program " + ex + "\n");
            }
        }
    }
    
}
