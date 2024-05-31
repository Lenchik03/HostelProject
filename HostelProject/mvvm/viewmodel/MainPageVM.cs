using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using HostelProject.mvvm.model;
using HostelProject.mvvm.view;
using Type = HostelProject.mvvm.model.Type;

namespace HostelProject.mvvm.viewmodel
{
    public class MainPageVM : BaseVM //главное меню
    {

        public VmCommand Create { get; set; } // кнопка "Добавить гостя"
        public VmCommand Edit { get; set; } // кнопка "Редактировать гостя"
        public VmCommand Delete { get; set; } // кнопка "Выселить гостя"
        public VmCommand CreateRoom { get; set; } // кнопка "Добавить номер"
        public VmCommand CreateYear { get; set; }
        public VmCommand EditRoom { get; set; } // кнопка "Редактировать номер"
        public VmCommand RemoveRoom { get; set; } // кнопка "Удалить номер"
        public VmCommand DeleteType { get; set; } // кнопка "Удалить тип"
        public VmCommand CreateType { get; set; } // кнопка "Добавить тип"
        public VmCommand EditType { get; set; } // кнопка "Редактировать тип"


        private MainVM mainVM;
        private string searchText = ""; // текст поиска
        private ObservableCollection<Guest> guests;
        private ObservableCollection<Guest> allguests;
        private ObservableCollection<Month> months;
        private ObservableCollection<Room> rooms;
        private ObservableCollection<Year> years;
        private ObservableCollection<model.Type> types;
        public ObservableCollection<Room> AllRooms { get; set; }
        public ObservableCollection<Room> AllAllRooms { get; set; }
        public ObservableCollection<Month> AllMonth { get; set; }
        public ObservableCollection<Year> AllYears { get; set; }


        private Room selectedRoom;


        public Room SelectedRoom // выбранный из фильтра номер, а также выбранный из списка номеров номер
        {
            get => selectedRoom;
            set
            {
                selectedRoom = value;
                Signal();
                Search();
                SearchMonth();
                SearchOutMonth();
                SearchYear();
            }
        }

        private Year selectedInYear;


        public Year SelectedInYear
        {
            get => selectedInYear;
            set
            {
                selectedInYear = value;
                Signal();
                SearchYear();
            }
        }

        private Month selectedInMonth;


        public Month SelectedInMonth
        {
            get => selectedInMonth;
            set
            {
                selectedInMonth = value;
                Signal();
                SearchMonth();
            }
        }
        private Month selectedOutMonth;
        public Month SelectedOutMonth
        {
            get => selectedOutMonth;
            set
            {
                selectedOutMonth = value;
                Signal();
                SearchOutMonth();
            }
        }


        public string SearchText // текст, по которому мы ищем гостя
        {
            get => searchText;
            set
            {
                searchText = value;
                Search();
                SearchMonth();
                SearchOutMonth();
            }
        }

        public Guest SelectedGuest { get; set; } // выбранный из списка гостей гость
        public Type SelectedType { get; set; } // выбранный из списка гостей гость
        public ObservableCollection<Guest> Guests // список гостей
        {
            get => guests;
            set
            {
                guests = value;
                Signal();
            }
        }

        public ObservableCollection<Year> Years // список гостей
        {
            get => years;
            set
            {
                years = value;
                Signal();
            }
        }

        public ObservableCollection<Guest> AllGuests // список гостей
        {
            get => allguests;
            set
            {
                allguests = value;
                Signal();
            }
        }

        public ObservableCollection<Room> Rooms // список номеров
        {
            get => rooms;
            set
            {
                rooms = value;
                Signal();
            }
        }

        public ObservableCollection<model.Type> Types // список типов

        {
            get => types;
            set
            {
                types = value;
                Signal();
            }
        }


        public MainPageVM()
        {
            // получение списка типов для фильтра
            string sql = "SELECT r.room_id, r.room_number, r.price, r.capacity_id, r.type_id, r.people_count, r.capacity, c.title as capacitytitle, t.title as type FROM rooms r, capacities c, types t WHERE r.capacity_id = c.capacity_id AND r.type_id = t.type_id AND r.del = 0;";
            AllRooms = new ObservableCollection<Room>(RoomRepository.Instance.GetAllRooms(sql));
            AllRooms.Insert(0, new Room { Id = 0, RoomNumber = "Все номера" });
            SelectedRoom = AllRooms[0];

            string sql3 = "SELECT r.room_id, r.room_number, r.price, r.capacity_id, r.type_id, r.people_count, r.capacity, c.title as capacitytitle, t.title as type FROM rooms r, capacities c, types t WHERE r.capacity_id = c.capacity_id AND r.type_id = t.type_id;";
            AllAllRooms = new ObservableCollection<Room>(RoomRepository.Instance.GetAllRooms(sql3));
            AllAllRooms.Insert(0, new Room { Id = 0, RoomNumber = "Все номера" });
            SelectedRoom = AllAllRooms[0];

            AllYears = new ObservableCollection<Year>(YearRepository.Instance.GetAllYears());
            AllYears.Insert(0, new Year { ID = 0, Title = "Все года" });
            SelectedInYear = AllYears[0];

            AllMonth = new ObservableCollection<Month>(MonthRepository.Instance.GetAllMonth());
            AllMonth.Insert(0, new Month { ID = 0, Title = "Все месяца"});
            SelectedInMonth = AllMonth[0];

            AllMonth = new ObservableCollection<Month>(MonthRepository.Instance.GetAllMonth());
            AllMonth.Insert(0, new Month { ID = 0, Title = "Все месяца"});
            SelectedOutMonth = AllMonth[0];

            // получение списка всех гостей
            string sql1 = "SELECT g.guest_id, g.name, g.secondname, g.phone_number, g.room_id, g.in_date, g.out_date, r.room_number as room_number FROM guests g, rooms r WHERE g.room_id = r.room_id AND g.out_date Is NULL;";
            Guests = new ObservableCollection<Guest>(GuestRepository.Instance.GetAllGuests(sql1));

            string sql2 = "SELECT g.guest_id, g.name, g.secondname, g.phone_number, g.room_id, g.in_date, g.out_date, r.room_number as room_number FROM guests g, rooms r WHERE g.room_id = r.room_id;";
            AllGuests = new ObservableCollection<Guest>(GuestRepository.Instance.GetAllGuests(sql2));

            // получение списка всех номеров
            Rooms = new ObservableCollection<Room>(RoomRepository.Instance.GetAllRooms(sql));

            // получение списка всех типов
            Types = new ObservableCollection<model.Type>(TypeRepository.Instance.GetAllTypes());

            Years = new ObservableCollection<Year>(YearRepository.Instance.GetAllYears());


            // команда на открытие страницы добавления гостя
            Create = new VmCommand(() =>
            {
                mainVM.CurrentPage = new SettingGuestPage(mainVM);
            });

            // команда на открытие страницы редактирования гостя, если был выбран гость из списка
            Edit = new VmCommand(() => {
                if (SelectedGuest == null)
                    return;
                mainVM.CurrentPage = new SettingGuestPage(mainVM, SelectedGuest);
            });

            // команда на удаление гостя
            Delete = new VmCommand(() =>
            {
                if (SelectedGuest == null)
                    return;

                if (MessageBox.Show("Выселение гостя", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    GuestRepository.Instance.Remove(SelectedGuest);
                    //RoomRepository.Instance.UpdatePeopleCountMinus(SelectedRoom);
                    Guests.Remove(SelectedGuest); // удаление гостя из коллекции


                }
            });

            // команда на добаления номера
            CreateRoom = new VmCommand(() =>
            {
                // открытие страницы добавления номера
                mainVM.CurrentPage = new SettingRoomPage(mainVM);

            });

            // команда на редактирование номера
            EditRoom = new VmCommand(() =>
            {
                // открытие страницы редактирования выбранного номера
                mainVM.CurrentPage = new SettingRoomPage(mainVM, SelectedRoom);
            });

            // команда на удаление выбранного номера
            RemoveRoom = new VmCommand(() => {
                if (SelectedRoom == null)
                    return;

                if (MessageBox.Show("Удаление номера", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    RoomRepository.Instance.Remove(SelectedRoom);
                    RoomRepository.Instance.UpdatePeopleCountMinus(SelectedRoom);

                    // обновление списка номеров
                    Rooms = new ObservableCollection<Room>(RoomRepository.Instance.GetAllRooms(sql));
                }
            });


            // команда на удаление типа
            CreateType = new VmCommand(() =>
            {
                // открытие страницы добавления типа
                mainVM.CurrentPage = new SettingTypePage(mainVM);
            });

            CreateYear = new VmCommand(() =>
            {
                // открытие страницы добавления типа
                mainVM.CurrentPage = new SettingYearPage(mainVM);
            });

            // команда на редактирование выбранного типа
            EditType = new VmCommand(() => {
                if (SelectedType == null)
                    return;
                // открытие страницы редактирования типа
                mainVM.CurrentPage = new SettingTypePage(mainVM, SelectedType);
            });

            // команда на удаление выбранного типа
            DeleteType = new VmCommand(() =>
            {
                if (SelectedType == null)
                    return;

                if (MessageBox.Show("Удаление типа комнаты", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    TypeRepository.Instance.Remove(SelectedType);

                    // обновление списка типов
                    Types = new ObservableCollection<model.Type>(TypeRepository.Instance.GetAllTypes());
                }
            });
        }

        internal void SetMainVM(MainVM mainVM)
        {
            this.mainVM = mainVM;
        }

        private void Search()
        {
            // список гостей после фильтрации или поиска
            Guests = new ObservableCollection<Guest>(
                GuestRepository.Instance.Search(SearchText, SelectedRoom));
        }
        private void SearchMonth()
        {
            // список гостей после фильтрации по месяцам заселения
            AllGuests = new ObservableCollection<Guest>(
                GuestRepository.Instance.SearchMonth(SearchText, SelectedRoom, SelectedInMonth));
        }
        private void SearchOutMonth()
        {
            // список клиентов после фильтрации по месяцам выселения
            AllGuests = new ObservableCollection<Guest>(
                GuestRepository.Instance.SearchOutMonth(SearchText, SelectedRoom, SelectedOutMonth));
        }

        private void SearchYear()
        {
            
            AllGuests = new ObservableCollection<Guest>(
                GuestRepository.Instance.SearchYear(SearchText, SelectedRoom, SelectedInYear));
        }
    }
}