using System;
using System.Globalization;

namespace Questao1
{
    public interface IOperacoes
    {

        void Deposito(double quantia);
        void Saque(double quantia);
    }

    public static class Taxa
    {
        public const double taxa_bancaria = 3.5;
    }

    public class Operacoes : IOperacoes
    {
        private double Saldo;
        private double TaxaBancaria;

        public Operacoes(double saldoInicial, double taxaBancaria)
        {
            Saldo = saldoInicial;
            TaxaBancaria = taxaBancaria;
        }

        public void Deposito(double quantia)
        {
            if (quantia <= 0)
            {
                throw new ArgumentException("O valor do depósito deve ser maior que zero.", nameof(quantia));
            }

            Saldo += quantia;
        }

        public void Saque(double quantia)
        {
            if (quantia <= 0)
            {
                throw new ArgumentException("O valor do saque deve ser maior que zero.", nameof(quantia));
            }

            if (quantia + TaxaBancaria > Saldo)
            {
                throw new InvalidOperationException("Saldo insuficiente para realizar o saque.");
            }

            Saldo -= quantia + TaxaBancaria;
        }

        public double GetSaldo()
        {
            return Saldo;
        }
    }

    public class ContaBancaria
    {
        public int Numero { get; }
        public string Titular { get; set; }
        private Operacoes operacoes;

        // Construtor para inicialização com depósito inicial opcional
        public ContaBancaria(int numero, string titular, double depositoInicial = 0.0)
        {
            Numero = numero;
            Titular = titular;
            operacoes = new Operacoes(depositoInicial, Taxa.taxa_bancaria);
        }

        // Método para formatar a representação da conta em string
        public override string ToString()
        {
            return $"Conta {Numero}, Titular: {Titular}, Saldo: $ {operacoes.GetSaldo().ToString("F2")}";
        }

        // Método para realizar um depósito na conta
        public void Deposito(double quantia)
        {
            operacoes.Deposito(quantia);
        }

        // Método para realizar um saque na conta
        public void Saque(double quantia)
        {
            operacoes.Saque(quantia);
        }
    }
}
