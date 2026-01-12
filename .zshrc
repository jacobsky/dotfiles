# Set the directory we want to store zinit
ZINIT_HOME="${XDF_DATA_HOME:-${HOME}/.local/share}/zinit/zinit.git"

if [ ! -d "$ZINIT_HOME" ]; then
    mkdir -p "$(dirname $ZINIT_HOME)"
    git clone https://github.com/zdharma-continuum/zinit.git "$ZINIT_HOME"
fi

export GOPATH=$HOME/go
export PATH="$PATH:/home/jacobsky/.cargo/bin/:/usr/local/sbin:/usr/local/bin:$GOPATH/bin:/home/jacobsky/.local/bin"
export PATH="$PATH:/mnt/c/Tools/"
alias vim=nvim
alias lg=lazygit
alias lzd=lazydocker
alias lsql=lazysql
export EDITOR=nvim
export VISUAL=$EDITOR
source "${ZINIT_HOME}/zinit.zsh"

zinit ice depth=1; 
zinit light zsh-users/zsh-syntax-highlighting
zinit light zsh-users/zsh-completions
zinit light zsh-users/zsh-autosuggestions
zinit light Aloxaf/fzf-tab
# load zsh-completions
autoload -U compinit && compinit

# To customize prompt, run `p10k configure` or edit ~/.p10k.zsh.
[[ ! -f ~/.p10k.zsh ]] || source ~/.p10k.zsh

bindkey -e
bindkey '^H' backward-kill-word
bindkey '^[[3~' delete-char
bindkey '^[[3;5~' kill-word
bindkey '^p' history-search-backward
bindkey '^n' history-search-forward
bindkey '^[[1;5C' forward-word
bindkey '^[[1;5D' backward-word

HISTSIZE=5000
HISTFILE=~/.zsh_history
SAVEHIST=$HISTSIZE
HISTDUP=erase
setopt appendhistory
setopt sharehistory
setopt hist_ignore_space
setopt hist_ignore_all_dups
setopt hist_save_no_dups
setopt hist_ignore_dups
setopt hist_find_no_dups

# Completion styles
zstyle ':completion:*' matcher-list 'm:{a-z}={A-Za-z}'
zstyle ':completion:*' list-colors "${(s.:.)LS_COLORS}"
zstyle ':completion:*' menu no
zstyle 'fzf-tab:complete:cd:*' fzf-preview 'ls --color $realpath'

alias ls='ls --color'
alias ll='ls -l --color'
alias la='ls -la --color'
# Shell integration

[ -f ~/.fzf.zsh ] && source ~/.fzf.zsh
#eval "$(oh-my-posh init zsh --config ~/.config/oh-my-posh/zen.omp.toml)"
eval "$(oh-my-posh init zsh --config ~/.config/oh-my-posh/tokyonight_storm.omp.toml)"
eval "$(fzf --zsh)"
eval "$(zoxide init --cmd cd zsh)"
