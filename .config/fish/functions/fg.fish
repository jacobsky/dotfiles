function fg --description "Smart fg with fzf picker for multiple jobs"
    # 1. If arguments were passed (e.g., fg %2), pass straight to builtin fg
    if test (count $argv) -gt 0
        builtin fg $argv
        return
    end

    set -l job_list (jobs)
    set -l job_count (count $job_list)

    if test $job_count -eq 0
        echo "fg: no current jobs"
        return 1
    else if test $job_count -eq 1
        # 2. Only one job suspended: resume immediately with zero friction
        builtin fg
    else
        # 3. Multiple jobs suspended: pick with fzf
        set -l job_id (printf '%s\n' $job_list | fzf --height=20% --layout=reverse --border=none --prompt="fg > " | awk '{print $1}')
        if test -n "$job_id"
            builtin fg %$job_id
        end
    end
end
