using BuildPCServrice;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
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

namespace TelephoneSpravochnik
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public enum TableType
    {
        Phone_category, Districts, Lgotnaya_category, Abonents
    }
    public partial class MainWindow : Window
    {
        DbService db; //Для работы с базой
        TableType currentTableType; //Хранит текущую открытую таблиц
        public MainWindow()
        {
            new LoginWindow().ShowDialog();
            InitializeComponent();

            //Инициализация и обновление списка видеокарт
            db = new DbService();
            currentTableType = TableType.Abonents;
            RefreshTable(currentTableType);
        }

        private void RefreshTable(TableType tt)
        {
            db = new DbService();
            CollectionViewSource vs = new CollectionViewSource();
            switch (tt)
            {
                case TableType.Phone_category:
                    db.Phone_category.Load();

                    vs.Source = db.Phone_category.Local;
                    this.Phone_categoryTable.ItemsSource = vs.View;
                    this.Phone_categoryTable.AddingNewItem += (sender, e) => e.NewItem = new Phone_category() { Name = "<новый>" };

                    Views.Phone_categoryView = vs;
                    break;
                case TableType.Districts:
                    db.Districts.Load();

                    vs.Source = db.Districts.Local;
                    this.DistrictTable.ItemsSource = vs.View;
                    this.DistrictTable.AddingNewItem += (sender, e) => e.NewItem = new District() { Name = "<Введите район>" };

                    Views.DistrictsView = vs;
                    break;
                case TableType.Lgotnaya_category:
                    db.Lgotnaya_category.Load();

                    vs.Source = db.Lgotnaya_category.Local;
                    this.Lgotnaya_categoryTable.ItemsSource = vs.View;
                    this.Lgotnaya_categoryTable.AddingNewItem += (sender, e) => e.NewItem = new Lgotnaya_category() { Name = "<Введите льготную категорию>" };

                    Views.Lgotnaya_categoryView = vs;
                    break;
                case TableType.Abonents:
                    db.Abonents.Load();

                    vs.Source = db.Abonents.Local;
                    this.AbonentsTable.AddingNewItem += (sender, e) => e.NewItem = new Abonent() { FIO = "<Введите имя>", Phone_Number = "<Введите номер телефона>", Date = DateTime.Now, Adress = "<Введите адрес>", DistrictsID = 0, Lgotnaya_categoryID = 0, Phone_categoryID = 0 };
                    this.AbonentsTable.ItemsSource = vs.View;
                    this.colDistrict.ItemsSource = db.Districts.ToArray();
                    this.colLgotnaya_category.ItemsSource = db.Lgotnaya_category.ToArray();
                    this.colPhone_category.ItemsSource = db.Phone_category.ToArray();

                    Views.AbonentsView = vs;
                    break;
            }
        }
        private void TabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TabItem ti)
            {
                TableType old = currentTableType;

                string header = ti.Header.ToString();
                if (header == "Льготные Категории")
                    currentTableType = TableType.Lgotnaya_category;
                else if (header == "Районы")
                    currentTableType = TableType.Districts;
                else if (header == "Абоненты")
                    currentTableType = TableType.Abonents;
                else if (header == "Категории Телефонов")
                    currentTableType = TableType.Phone_category;

                if (currentTableType != old)
                    RefreshTable(currentTableType);
            }
        }
        private void SaveChanges(TableType tt)
        {
            db.SaveChanges();

            DataGrid currTable = null;
            switch (tt)
            {
                case TableType.Districts:
                    currTable = DistrictTable;
                    break;
                case TableType.Lgotnaya_category:
                    currTable = Lgotnaya_categoryTable;
                    break;
                case TableType.Abonents:
                    currTable = AbonentsTable;
                    break;
                case TableType.Phone_category:
                    currTable = Phone_categoryTable;
                    break;
            }

            int si = currTable.SelectedIndex;
            RefreshTable(tt);
            currTable.SelectedIndex = si;
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            SaveChanges(currentTableType);
        }

        private void DeleteRecord(TableType tt)
        {
            switch (tt)
            {
                case TableType.Abonents:
                    if (AbonentsTable.SelectedItem is Abonent a)
                        db.Abonents.Local.Remove(a);
                    else
                        MessageBox.Show("Данный абонент уже существует!",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
                case TableType.Lgotnaya_category:
                    if (Lgotnaya_categoryTable.SelectedItem is Lgotnaya_category l)
                        db.Lgotnaya_category.Local.Remove(l);
                    else
                        MessageBox.Show("Данная льготная категория уже существует!",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
                case TableType.Phone_category:
                    if (Phone_categoryTable.SelectedItem is Phone_category p)
                        db.Phone_category.Local.Remove(p);
                    else
                        MessageBox.Show("Данная категория телефона уже присутствует!",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
                case TableType.Districts:
                    if (DistrictTable.SelectedItem is District v)
                        db.Districts.Local.Remove(v);
                    else
                        MessageBox.Show("Данный район уже существует!",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
            }
        }

        private void CancelChangesButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshTable(currentTableType);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteRecord(currentTableType);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            switch (currentTableType)
            {
                case TableType.Districts:
                    Views.DistrictsView.Filter += (o, ea) =>
                    {
                        if (ea.Item is District v)
                        {
                            string name = v.Name.ToLower();

                            if (name.Contains(DistSearchName.Text.ToLower()))

                            {
                                ea.Accepted = true;
                            }
                            else
                            {
                                ea.Accepted = false;
                            }
                        }
                    };
                    break;
                case TableType.Lgotnaya_category:
                    Views.Lgotnaya_categoryView.Filter += (o, ea) =>
                    {
                        if (ea.Item is Lgotnaya_category p)
                        {
                            string name = p.Name.ToLower();

                            if (name.Contains(LgotSearchName.Text.ToLower()))

                            {
                                ea.Accepted = true;
                            }
                            else
                            {
                                ea.Accepted = false;
                            }
                        }
                    };
                    break;
                case TableType.Abonents:
                    Views.AbonentsView.Filter += (o, ea) =>
                    {
                        if (ea.Item is Abonent p)
                        {
                            string name = p.FIO.ToLower();

                            if (name.Contains(AbonentSearchName.Text.ToLower()))

                            {
                                ea.Accepted = true;
                            }
                            else
                            {
                                ea.Accepted = false;
                            }
                        }
                    };
                    break;
            }
        }

        private void CancelSearchButton_Click(object sender, RoutedEventArgs e)
        {
            switch (currentTableType)
            {
                case TableType.Districts:
                    Views.DistrictsView.Filter += (o, ea) => ea.Accepted = true;

                    DistSearchName.Text = "";

                    break;
                case TableType.Lgotnaya_category:
                    Views.Lgotnaya_categoryView.Filter += (o, ea) => ea.Accepted = true;

                    LgotSearchName.Text = "";

                    break;
                case TableType.Abonents:
                    Views.AbonentsView.Filter += (o, ea) => ea.Accepted = true;

                    AbonentSearchName.Text = "";
                    AbonentSearchDist.Text = "";
                    AbonentSearchLgot.Text = "";
                    AbonentSearchPhone.Text = "0";
                    break;
            }
        }
        Report report;
        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            report = new Report();
            switch (currentTableType)
            {
                case TableType.Districts:
                    report.District(Views.DistrictsView.Source as IList<District>);
                    break;
                case TableType.Lgotnaya_category:
                    report.Lgotnaya_category(Views.Lgotnaya_categoryView.Source as IList<Lgotnaya_category>);
                    break;
                case TableType.Abonents:
                    report.Abonents(Views.AbonentsView.Source as IList<Abonent>);
                    break;
                case TableType.Phone_category:
                    report.Phone_category(Views.Phone_categoryView.Source as IList<Phone_category>);
                    break;
            }
        }
    }
}
