﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Adega
{
    public partial class FormAdega : Form
    {
        public FormAdega()
        {
            InitializeComponent();
        }
        private MySqlConnectionStringBuilder conexaoBanco()
        {
            MySqlConnectionStringBuilder conexaoBD = new MySqlConnectionStringBuilder();
            conexaoBD.Server = "localhost";
            conexaoBD.Database = "adega";
            conexaoBD.UserID = "root";
            conexaoBD.Password = "";
            conexaoBD.SslMode = 0;
            return conexaoBD;
        }
        private void btLimpar_Click(object sender, EventArgs e)
            {
            limparCampos();
            }

            private void limparCampos()
            {
                tbID.Clear();
                tbAno.Clear();
                tbNome.Clear();
                tbCategoria.Clear();
                tbDescricao.Clear();
            }

            private void FormAdega_Load(object sender, EventArgs e)
        {
            atualizaGrid();
        }

        private void atualizaGrid()
        {
            MySqlConnectionStringBuilder conexaoBD = conexaoBanco();
            MySqlConnection realizaConexacoBD = new MySqlConnection(conexaoBD.ToString());
            try
            {
                realizaConexacoBD.Open();

                MySqlCommand comandoMySql = realizaConexacoBD.CreateCommand();
                comandoMySql.CommandText = "SELECT * FROM vinho WHERE ativoVinho = 1";
                MySqlDataReader reader = comandoMySql.ExecuteReader();

                dgAdega.Rows.Clear();

                while (reader.Read())
                {
                    DataGridViewRow row = (DataGridViewRow)dgAdega.Rows[0].Clone();//FAZ UM CAST E CLONA A LINHA DA TABELA
                    row.Cells[0].Value = reader.GetInt32(0);//ID
                    row.Cells[1].Value = reader.GetString(1);//NOME
                    row.Cells[2].Value = reader.GetString(2);//DESCRICAO
                    row.Cells[3].Value = reader.GetString(3);//CATEGORIA
                    row.Cells[4].Value = reader.GetString(4);//ANO
                    dgAdega.Rows.Add(row);//ADICIONO A LINHA NA TABELA
                }

                realizaConexacoBD.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! ");
                Console.WriteLine(ex.Message);
            }
        }
       
        private void btInserir_Click(object sender, EventArgs e)
        {
            MySqlConnectionStringBuilder conexaoBD = conexaoBanco();
            MySqlConnection realizaConexacoBD = new MySqlConnection(conexaoBD.ToString());
            try
            {
                realizaConexacoBD.Open();

                MySqlCommand comandoMySql = realizaConexacoBD.CreateCommand();

                comandoMySql.CommandText = "INSERT INTO vinho (nomeVinho,categoriaVinho,descricaoVinho,anoVinho) " +
                    "VALUES('" + tbNome.Text + "', '" + tbCategoria.Text + "','" + tbDescricao.Text + "', " + Convert.ToInt16(tbAno.Text) + ")";
                comandoMySql.ExecuteNonQuery();

                realizaConexacoBD.Close();
                MessageBox.Show("Inserido com sucesso");
                atualizaGrid();
                limparCampos();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

       

            private void dgAdega_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgAdega.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                dgAdega.CurrentRow.Selected = true;
                //preenche os textbox com as células da linha selecionada
                tbNome.Text = dgAdega.Rows[e.RowIndex].Cells["colNome"].FormattedValue.ToString();
                tbCategoria.Text = dgAdega.Rows[e.RowIndex].Cells["colCategoria"].FormattedValue.ToString();
                tbDescricao.Text = dgAdega.Rows[e.RowIndex].Cells["colDescricao"].FormattedValue.ToString();
                tbAno.Text = dgAdega.Rows[e.RowIndex].Cells["colAno"].FormattedValue.ToString();
                tbID.Text = dgAdega.Rows[e.RowIndex].Cells["colID"].FormattedValue.ToString();
            }
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            MySqlConnectionStringBuilder conexaoBD = conexaoBanco();
            MySqlConnection realizaConexacoBD = new MySqlConnection(conexaoBD.ToString());
            try
            {
                realizaConexacoBD.Open();

                MySqlCommand comandoMySql = realizaConexacoBD.CreateCommand();
                comandoMySql.CommandText = "UPDATE vinho SET nomeVinho = '" + tbNome.Text + "', " +
                    "descricaoVinho = '" + tbDescricao.Text + "', " +
                    "categoriaVinho = '" + tbCategoria.Text + "', " +
                    "anoVinho = " + Convert.ToInt16(tbAno.Text) +
                    " WHERE idVinho = " + tbID.Text + "";
                comandoMySql.ExecuteNonQuery();

                realizaConexacoBD.Close();
                MessageBox.Show("Atualizado com sucesso");
                atualizaGrid();
                limparCampos();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }

        private void btnDeletar_Click(object sender, EventArgs e)
        {
            MySqlConnectionStringBuilder conexaoBD = conexaoBanco();
            MySqlConnection realizaConexacoBD = new MySqlConnection(conexaoBD.ToString());
            try
            {
                realizaConexacoBD.Open(); 

                MySqlCommand comandoMySql = realizaConexacoBD.CreateCommand(); 
               
                comandoMySql.CommandText = "UPDATE vinho SET ativovinho = 0 WHERE idVinho = " + tbID.Text + "";

                comandoMySql.ExecuteNonQuery();

                realizaConexacoBD.Close(); 
                MessageBox.Show("Removido com sucesso"); 
                atualizaGrid();
                limparCampos();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine(ex.Message);
            }

        }
    }
}
