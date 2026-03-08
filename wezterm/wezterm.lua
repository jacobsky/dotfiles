-- Pull in the wezterm API
local wezterm = require("wezterm")

-- This will hold the configuration.
local config = wezterm.config_builder()

-- This is where you actually apply your config choices.
config.default_prog = {
	"wsl.exe",
	"--distribution-id",
	"{df916528-c765-441e-a5c1-c3facb3c96b0}",
	"--cd",
	"~",
}
-- For example, changing the initial geometry for new windows:
config.initial_cols = 120
config.initial_rows = 28

-- or, changing the font size and color scheme.

-- Font settings
config.font = wezterm.font("Hack Nerd Font")
config.font_size = 12

-- Window setting/ appearance
config.enable_tab_bar = true
config.color_scheme = "Tokyo Night Storm"
config.use_fancy_tab_bar = false

-- Finally, return the configuration to wezterm:
return config
