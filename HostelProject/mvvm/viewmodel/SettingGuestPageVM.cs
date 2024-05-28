﻿using HostelProject.mvvm.model;
using HostelProject.mvvm.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HostelProject.mvvm.viewmodel
{
    public class SettingGuestPageVM: BaseVM
    {
        private Room selectedRoom;
        public VmCommand Save { get; set; } // кнопка "Сохранить"
        public VmCommand Delete { get; set; }
        MainVM mainVM;

        private Guest guest = new();

        public Guest Guest // гость, которого мы добавляем или редактируем
        {
            get => guest;
            set
            {
                guest = value;
                Signal();
            }
        }

        public Room SelectedRoom // выбранный номер из ComboBox`a
        {
            get => selectedRoom; set
            {
                selectedRoom = value;
                Signal();
            }
        }
        public List<Room> AllRooms { get; set; } // список типов (ComboBox типов)

        public SettingGuestPageVM()
        {
            // получение списка типов
            AllRooms = (List<Room>?)RoomRepository.Instance.GetAllRooms();

            //команда на добавление в базу или обновление типов в базе
            Save = new VmCommand(() => {

                // если из Combobox`а НЕ выбран тип, то по умолчанию будет выбран первый тип
                Guest.RoomId = SelectedRoom?.Id ?? 1;

                //if (SelectedRoom == null)
                //{
                //    //MessageBox.Show("Укажите номер");
                //}
                //else
                //

                    if (Guest.Id == 0)
                    {
                        if (SelectedRoom.PeopleCount < SelectedRoom.Capacity)
                        {

                            RoomRepository.Instance.UpdatePeopleCount(SelectedRoom);
                            GuestRepository.Instance.Add(Guest); // добавление гостя
                            mainVM.CurrentPage = new MainPage(mainVM);
                        }
                        else
                        {
                            MessageBox.Show("Номер занят!!!");
                            mainVM.CurrentPage = new SettingGuestPage(mainVM, Guest);
                        }
                    }

                    else
                    {
                        //SelectedRoom.Id = Guest.RoomId;
                        GuestRepository.Instance.Update(Guest); // если гость выбран из списка - редактирование гостя
                        mainVM.CurrentPage = new MainPage(mainVM);
                    }
                //}
                 // после успешного добавления или редактирования гостя, откроется главное меню
            });

            //Delete = new VmCommand(() => {
            //    if (Guest == null)
            //        return;

            //    if (MessageBox.Show("Выселение гостя", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            //    {
            //        //SelectedRoom.Id = Guest.RoomId;
            //        GuestRepository.Instance.Remove(Guest);
            //        //RoomRepository.Instance.UpdatePeopleCountMinus(SelectedRoom);
            //        /*Guests.Remove(SelectedGuest);*/ // удаление клиента из коллекции
            //        mainVM.CurrentPage = new MainPage(mainVM);


            //    }
            //});


        }


        internal void SetMainVM(MainVM mainVM)
        {
            this.mainVM = mainVM;
        }

        internal void SetEditGuest(Guest selectedGuest)
        {
            Guest = selectedGuest;
            SelectedRoom = AllRooms.FirstOrDefault(s => s.Id == guest.Id);
        }
    }
}
