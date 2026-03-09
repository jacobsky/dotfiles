# Dotfiles aka Configuration for zsh and associated tools

## Installation (and Prereqs)

### WSL Install (Windows Only)

Open up powershell and install wsl with the following command

```powershell
wsl.exe --install archlinux
wsl.exe --set-default archlinux
```

#### .wslconfig (highly recommended)

WSL has a tendency to consume at minimum 8GB of memory or _half_ of your total available memory. This includes your page file which windows tends to double. This means that even when idling, it will, by default, consume _all the memory_ even when your active processes are using considerably less.

In your `%UserProfile%` directory create a `.wslconfig` file

The following is a minimum configuration that you can use.

```bash
[wsl2]
memory=6GB # How much memory to assign to the WSL2 VM. (too low and you'll oom)
processors=2 # How many processors to assign to the WSL2 VM. (too low and parallel tasks may take longer)
swap=0 # Disable the swap to avoid pushing memory back and forth to the pagefile

# Used to help keep memory management under control
[experimental]
autoMemoryReclaim=gradual
```

### Package configuration

Follow `# Arch CFG` to configure all the packages assumed on the system _or_
ensure that `git` and `stow` are _at minimum_ installed.

Note: I have only tested this with my arch config, so it may not work out of the
box otherwise

### Installation

Clone the repo via git into $HOME directory and execute stow to create the
necessary symlinks.

```
git clone git@github.com/jacobsky/dotfiles.git
cd dotfiles
stow .
```

## Arch CFG

Configuring arch should be done via the following commands:

```bash
sudo useradd -m [username]
sudo passwd [username]
sudo usermod -aG wheel [username]
```

Finally, it will be necessary to edit the `/etc/sudoers` file to allow the wheel
members access

Once this is complete, ensure that Yet Another Yogurt (or paru)
is installed. Yay can be installed as per the following command

```bash
git clone https://aur.archlinux.org/yay-bin.git && cd yay-bin && makepkg -si
```

To help track the "generally used arch packages" that I use, I am maintaining
both the `pacman` and `yay` (AUR) packages as well as any custom installation
scripts for specific tools not found on the package managers which can be
installed via `scripts/install.sh`

When new additions are made, they can be automatically managed and added via the
`scripts/archpacbu.sh` script and running it from your home `~` directory.

## (manual) tmux package manager

This should be automatically taken care of by the dofiles, but if it hasn't, please install tmux plugin manager to

```
git clone https://github.com/tmux-plugins/tpm ~/.tmux/plugins/tpm
```

use <leader>+I to install all the plugins in an active tmux session

## 日本語IME

[im-select](https://github.com/daipeihust/im-select?tab=readme-ov-file)

Check the `im-select.nvim` to confirm the configuration.
