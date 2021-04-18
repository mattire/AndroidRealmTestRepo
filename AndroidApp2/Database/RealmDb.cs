using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Realms;
using RealmClone;
using System.Threading;

namespace AndroidApp2.Database
{
    class RealmDb
    {
        static public RealmConfiguration Config { get; private set; }

        private Realm _realm;

        public Thread RealmThread { get; private set; }

        private static readonly Lazy<RealmDb> lazy = new Lazy<RealmDb>(() => new RealmDb());

        public static RealmDb Instance { get { return lazy.Value; } }

        public Realm RealmInstance { get { return _realm; } }

        private RealmDb()
        {
            Init();
        }

        static public Realm GetRealmInstance() {
            return Realm.GetInstance(new RealmConfiguration()
            {
                SchemaVersion = 1,
                ShouldDeleteIfMigrationNeeded = true
            });
        }

        public void Init() {
            //using (var real)
            Config = new RealmConfiguration()
            {
                SchemaVersion = 1,
                ShouldDeleteIfMigrationNeeded = true
            };
            //Config.
            _realm = Realm.GetInstance(Config);
            RealmThread = Thread.CurrentThread;
            //_realm.Error
            _realm.RealmChanged += _realm_RealmChanged;

            //Message msg; 

            IDisposable msgToken = _realm.All<Message>().SubscribeForNotifications((sender, changes, errors) =>
            {
                if (changes != null) {
                    System.Diagnostics.Debug.WriteLine(changes.ModifiedIndices.Count());
                    System.Diagnostics.Debug.WriteLine(changes.DeletedIndices.Count());
                    System.Diagnostics.Debug.WriteLine(changes.InsertedIndices.Count());
                    System.Diagnostics.Debug.WriteLine("");
                }
            });
        }

        private void _realm_RealmChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(sender.GetType().Name);

        }

        public static void AddMessage(string text) {
            try
            {
                //_realm.Write(() =>
                //var r = _realm;
                var r = Realm.GetInstance(RealmDb.Config);

                //RealmThread.ExecutionContext.

                r.Write(() =>
                {
                    var m = new Message() { Text = text };
                    r.Add(m);
                });
                //AndroidApp2.UI.RecAdapter.Current.Update();

                //AndroidApp2.UI.RecAdapter.Current.NotifyDataSetChanged();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public Message GetMessage(string txtStartsWith)
        {
            var m1 = _realm.All<Message>().Where(m => m.Text.StartsWith(txtStartsWith)).First();
            var cm = m1.Clone();
            return cm;
        }

        public IQueryable<T> Get<T>() where T: RealmObject
        {
            return _realm.All<T>();
        }


    }
}