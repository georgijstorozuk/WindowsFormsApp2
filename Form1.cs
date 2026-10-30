using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity; // Обязательно для работы метода Load()
using WindowsFormsApp2.Model; // Подключаем вашу модель данных из папки Model

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        // Создаем экземпляр контекста базы данных (класс из вашей папки Model)
        Model1 model = new Model1();

        public Form1()
        {
            InitializeComponent();
        }

        // 1. Событие загрузки формы — заполняем источники данных
        private void Form1_Load(object sender, EventArgs e)
        {
            StartLoadData();
        }

        // 2. Событие нажатия на кнопку "Сохранить" в BindingNavigator
        private void usersBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        // 3. Событие нажатия на кнопку "Удалить" в BindingNavigator
        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        // 4. Событие нажатия на кнопку "Добавить новый" (+) в BindingNavigator
        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            // Автоматически выбираем первую роль в выпадающем списке для нового пользователя
            if (role_NameComboBox.Items.Count > 0)
            {
                role_NameComboBox.SelectedIndex = 0;
            }
        }

        // Метод загрузки данных из БД в интерфейс
        private void StartLoadData()
        {
            try
            {
                // Загружаем данные из таблицы Users в локальный кэш Entity Framework
                model.Users.Load();

                // Привязываем выпадающий список ролей к таблице Roles
                role_NameComboBox.DataSource = model.Roles.ToList();

                // Привязываем основную коллекцию пользователей к BindingSource
                usersBindingSource.DataSource = model.Users.Local.ToBindingList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Метод сохранения изменений обратно в базу данных
        private void SaveData()
        {
            try
            {
                this.Validate(); // Проверяем корректность ввода в полях формы
                usersBindingSource.EndEdit(); // Завершаем редактирование текущей записи
                usersBindingSource.ResetBindings(true); // Обновляем отображение данных на форме

                model.SaveChanges(); // Фиксируем все изменения (добавление/удаление/правки) в MS SQL
            }
            catch (Exception ex)
            {
                // Если произошла ошибка (например, из-за связей БД), выводим её и перезагружаем данные
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                StartLoadData();
            }
        }
    }
}

