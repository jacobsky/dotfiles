# Environment Variables
set -gx GOPATH $HOME/go
set -gx PATH $PATH ~/.cargo/bin/ /usr/local/sbin /usr/local/bin $GOPATH/bin ~/.local/bin
set -gx PATH $PATH /mnt/c/Tools/
set -gx EDITOR nvim
set -gx VISUAL nvim
set -g fish_vi_key_bindings

# Aliases
alias vim nvim
alias lg lazygit
alias lzd lazydocker
alias lsql lazysql
alias ls 'ls --color'
alias ll 'ls -l --color'
alias la 'ls -la --color'

# Shell integrations
fzf --fish | source
zoxide init --cmd cd fish | source
# Fisher plugin manager (install with: curl -sL https://git.io/fisher | source && fisher install jorgebucaran/fisher)

# Starship prompt (install with: curl -sS https://starship.rs/install.sh | sh)
starship init fish | source
