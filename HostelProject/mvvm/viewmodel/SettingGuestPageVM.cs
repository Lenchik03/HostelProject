using HostelProject.mvvm.model;
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
    public class SettingGuestPageVM : BaseVM
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
        public List<Room> AllRooms { get; set; } // список номеров (ComboBox типов)

        public SettingGuestPageVM()
        {
            // получение списка типов
            string sql = "SELECT r.room_id, r.room_number, r.price, r.capacity_id, r.type_id, r.people_count, r.capacity, c.title as capacitytitle, t.title as type FROM rooms r, capacities c, types t WHERE r.capacity_id = c.capacity_id AND r.type_id = t.type_id AND r.del = 0 AND r.capacity != r.people_count;";
            AllRooms = (List<Room>?)RoomRepository.Instance.GetAllRooms(sql);

            //команда на добавление в базу или обновление гостя в базе
            Save = new VmCommand(() => {

                // если из Combobox`а НЕ выбран тип, то по умолчанию будет выбран первый тип
                Guest.RoomId = SelectedRoom?.Id ?? 1;

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
                // после успешного добавления или редактирования гостя, откроется страница менеджера
            });
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
