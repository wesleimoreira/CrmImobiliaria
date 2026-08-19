using MudBlazor;

namespace CrmImobiliaria.Web.Theme
{
    // Paleta "corporativo/confiança": azul-marinho + verde-escuro sobre neutros claros.
    public static class CrmTheme
    {
        public static readonly MudTheme Theme = new()
        {
            PaletteLight = new PaletteLight
            {
                Primary = "#1B3A5F",
                Secondary = "#1B5E4A",
                AppbarBackground = "#12253A",
                AppbarText = "#FFFFFF",
                Background = "#F4F6F8",
                Surface = "#FFFFFF",
                TextPrimary = "#1F2937",
                DrawerBackground = "#FFFFFF",
                DrawerText = "#1F2937",
                DrawerIcon = "#1B3A5F"
            },
            LayoutProperties = new LayoutProperties
            {
                DefaultBorderRadius = "6px"
            }
        };
    }
}
