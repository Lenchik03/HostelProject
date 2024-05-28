using HostelProject.mvvm.model;
using HostelProject.mvvm.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HostelProject.mvvm.viewmodel
{
    public class RegistrationPageVM: BaseVM
    {
        private MainVM mainVM;
        public VmCommand Save { get; set; } // кнопка "Зарегистрироваться"
        private Admin selectedAdmin = new();
        public Admin SelectedAdmin // админ, которого мы регистрируем 
        {
            get => selectedAdmin;
            set
            {
                selectedAdmin = value;
                Signal();
            }
        }
        public RegistrationPageVM()
        {
            // Команда на создание нового админа
            Save = new VmCommand(() =>
            {
                SelectedAdmin.Password = Password; // присвоение админу нового пароля
                AdminRepository.Instance.Add(SelectedAdmin);

                // после успешной регистрации откроется страница авторизации
                SignInPage signInPage = new SignInPage();
                MainVM.Instance.CurrentPage = signInPage;

            });
        }

        internal void SetMainVM(MainVM mainVM)
        {
            this.mainVM = mainVM;
        }
        internal void SetPasswordBox(PasswordBox passwrdBox)
        {
            this.passwrdBox = passwrdBox;
        }

        private PasswordBox passwrdBox;
        public string Password // введенный админом пароль
        {
            get { return passwrdBox.Password; }
            set
            {
                passwrdBox.Password = value;
            }
        }
    }
}
