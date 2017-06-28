using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using SQLDemo.Data;
using Infragistics.Controls.DataSource;
using Infragistics.Core.Controls.DataSource;

namespace SQLDemo
{
    public partial class MainPage : ContentPage
    {
        private SQLiteAsyncConnection _connection;

        public MainPage(string dbPath)
        {
            _connection = new SQLiteAsyncConnection(dbPath);
            
            InitializeComponent();

            var dataSource = new SQLiteVirtualDataSource();
            dataSource.Connection = _connection;
            dataSource.TableExpression = "tracks left outer join albums on tracks.AlbumId = albums.AlbumId";
            dataSource.ProjectionType = typeof(Track);

            grid.ItemsSource = dataSource;
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(e.NewTextValue))
            {
                grid.FilterExpressions.Clear();
            }
            else
            {
                grid.FilterExpressions.Clear();
                grid.FilterExpressions.Add(FilterFactory.Build(
                    (f) =>
                    {
                        return f.Property("Name").Contains(e.NewTextValue)
                        .Or(f.Property("AlbumTitle").Contains(e.NewTextValue))
                        .Or(f.Property("Composer").Contains(e.NewTextValue));
                    }));
            }
        }
    }

}
