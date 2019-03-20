using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItVolga
{
    /// <summary>
    /// Логика взаимодействия для PageCalculator.xaml
    /// </summary>
    public partial class PageCalculator : Page
    {
        double sum;
        double dPercent;
        int srok;
        double payment;
        double sum_;
        double per;
        double dPer;
        double per_;
        double sum0;
        double time0;
        double dPercent0;
        double sum2;
        public PageCalculator()
        {
            InitializeComponent();
            srokBox.Text = "мес.";
            ProcentBox.Text = "% в год.";
            Money.Text = "₽";
            Value.Text = "% от суммы кредита";
        }
        private void BtnСalculate_Click(object sender, RoutedEventArgs e)
        {
            if (SummaText.Text == "" || ProcentText.Text == "" || SrokText.Text == "" || KomissiyText.Text == "" || datestart.Text == "")
            {
                MessageBox.Show("Empty field");
                return;
            }
            if (VidBox.SelectedIndex == 0)
            {
                try
                {
                    sum = double.Parse(SummaText.Text);
                    srok = Int32.Parse(SrokText.Text);
                    if (srokBox.Text == "год.")
                    {
                        srok *= 12;
                    }
                    dPercent = double.Parse(ProcentText.Text);
                    dPer = double.Parse(ProcentText.Text);
                    dPercent /= 1200;
                    payment = (sum * dPercent) / (1 - Math.Pow(1 + dPercent, -srok));
                }
                catch (FormatException)
                {
                    MessageBox.Show("Incorrect data entry format");
                }
                per = payment * srok - sum;
                CalcPayments();
            }
            else if (VidBox.SelectedIndex == 1)
            {
                try
                {
                    sum = double.Parse(SummaText.Text);
                    srok = Int32.Parse(SrokText.Text);
                    if (srokBox.Text == "год.")
                    {
                        srok *= 12;
                    }
                    dPercent = double.Parse(ProcentText.Text);
                    dPercent /= 1200;
                    sum_ = sum;
                    per_ = -sum;
                    sum0 = sum;
                    time0 = srok;
                    dPercent0 = dPercent;
                    sum2 = sum_;
                    for (int j = 0; j < srok; j++)
                    {
                        double sumMonth0 = sum0 / time0;
                        double perMonth0 = dPercent0 * sum2;
                        sum2 -= sumMonth0;
                        double total0 = sumMonth0 + perMonth0;
                        per_ = per_ + total0;
                    }
                    
                }
                catch (FormatException)
                {
                    MessageBox.Show("Incorrect data entry format");
                }
                CalcPayments2();
            }
            else
            {
                VidBox.Text = "No type selected";
            }
            
        }

        private void CalcPayments2()
        {
            DateTime payDay = Convert.ToDateTime(datestart.Text);
            payDay = payDay.AddMonths(1);
            double b = 0;
            List<PaymentsClass> item = new List<PaymentsClass>();
            Window1 w1 = new Window1();
            for (int i = 0; i < srok; i++)
            {
                b++;
                double sumMonth = sum / srok;
                double perMonth = dPercent * sum_;
                sum_ -= sumMonth;
                double total = sumMonth + perMonth;
                item.Add(new PaymentsClass
                {
                    Number = b.ToString(),
                    PaymentDate = payDay.ToShortDateString(),
                    PrincipalDebt = Math.Round(sumMonth, 2).ToString(),
                    PaymentAmount = Math.Round(total, 2).ToString(),
                    AccruedInterest = Math.Round(perMonth, 2).ToString(),
                    OutstandingBalance = Math.Round(sum_, 2).ToString()
                });
                w1.label1.Content = Math.Round(Convert.ToDouble(SummaText.Text) + per_, 2)+" ₽";
                w1.label2.Content = Math.Round(per_, 2) + " ₽";
                w1.label3.Content = Math.Round(per_ + (Convert.ToDouble(SummaText.Text) * Convert.ToDouble(KomissiyText.Text)) / 100) + " ₽";
                payDay = payDay.AddMonths(1);
            }
            w1.GridPayments.ItemsSource = item.ToList();
            w1.ShowDialog();
        }
        private void CalcPayments()
        {
            double b = 0;
            List<PaymentsClass> item = new List<PaymentsClass>();
            DateTime payDay = Convert.ToDateTime(datestart.Text);
            payDay = payDay.AddMonths(1);
            Window1 w1 = new Window1();
            for (int i = 0; i < srok; i++)
            {
                b++;
                double perMonth = sum * dPercent;
                double sumMonth = payment - perMonth;
                sum -= sumMonth;
                double total = sumMonth + perMonth;
                item.Add(new PaymentsClass
                {
                    Number = b.ToString(),
                    PaymentDate = payDay.ToShortDateString(),
                    PrincipalDebt = Math.Round(sumMonth, 2).ToString(),
                    PaymentAmount = Math.Round(total, 2).ToString(),
                    AccruedInterest = Math.Round(perMonth, 2).ToString(),
                    OutstandingBalance = Math.Round(sum, 2).ToString()
                });
                payDay = payDay.AddMonths(1);
                w1.label1.Content = Math.Round(Convert.ToDouble(SummaText.Text) + per, 2) + " ₽";
                w1.label2.Content = Math.Round(per, 2) + " ₽";
                w1.label3.Content = Math.Round(per + (Convert.ToDouble(SummaText.Text) * Convert.ToDouble(KomissiyText.Text)) / 100) + " ₽";
            }
            w1.GridPayments.ItemsSource = item.ToList();
            w1.ShowDialog();
        }

    }
}

