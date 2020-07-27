namespace CraftKnife
{
    class Global
    {
        public static bool can_use_commands = false;
        public static float time_to_craft = 120.0f;
        public static float distance_to_cut = 1.0f;
        public static int damage = 20;

        public static string _success_start = "Вы мастерите заточку. Не двигайтесь в течении: ";
        public static string _toolong = "Цель слишком далеко, либо находится вплотную к вам";
        public static string _nothaveknife = "У вас нет заточки";
        public static string _success_cut = "Заточка сломалась. Вы порезали ";

        public static string _already_craft = "Вы уже мастерите заточку";
        public static string _already_have_knife = "У вас уже есть заточка";

        public static string _not_have_flashlight = "У вас нет фонарика";

        public static string _isscp = "Вы не нанесли урона, так это SCP объект";
    }
}