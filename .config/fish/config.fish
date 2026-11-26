# Environment Variables
set -gx GOPATH $HOME/go
set -gx PATH $PATH ~/.cargo/bin/ /usr/local/sbin /usr/local/bin $GOPATH/bin ~/.local/bin
set -gx PATH $PATH /mnt/c/Tools/
set -gx EDITOR nvim
set -gx VISUAL nvim
set -g fish_key_bindings fish_vi_key_bindings

# Aliases
alias vim nvim
alias lg lazygit
alias lzd lazydocker
alias lsql lazysql
alias ls eza
alias ll 'eza -l'
alias la 'eza -la'
alias sg ast-grep

# Shell integrations
fzf --fish | source
zoxide init --cmd cd fish | source
# Fisher plugin manager (install with: curl -sL https://git.io/fisher | source && fisher install jorgebucaran/fisher)

# Starship prompt (install with: curl -sS https://starship.rs/install.sh | sh)
starship init fish | source

# 1. Define a minimal prompt function used when context hasn't changed
function _minimal_prompt
    set -l status_color green
    if test $argv[1] -ne 0
        set status_color red
    end
    set_color $status_color
    echo -n "❯ "
    set_color normal
end

# 2. Wrap Starship's fish_prompt to only render the full header when state changes
function _init_smart_starship --on-event fish_prompt
    # Run once to capture Starship's original fish_prompt function
    functions -e _init_smart_starship
    functions -c fish_prompt _starship_full_prompt

    function fish_prompt
        set -l last_status $status

        # Capture current directory, jj working-copy change ID (if in a jj repo), and job count
        set -l jj_rev ""
        if test -d .jj; or jj root >/dev/null 2>&1
            set jj_rev (jj log -r @ --no-graph -T 'change_id.short() ++ bookmarks' 2>/dev/null)
        end
        set -l job_count (count (jobs))
        set -l current_state "$PWD|$jj_rev|$job_count"

        # If state matches the previous prompt, print only the bare character
        if test "$current_state" = "$_last_prompt_state"
            _minimal_prompt $last_status
        else
            set -g _last_prompt_state "$current_state"
            # Pass the original exit status back into Starship
            fish -c "exit $last_status" 2>/dev/null
            _starship_full_prompt
        end
    end
end
