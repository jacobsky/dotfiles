# Dotfiles aka Configuration for zsh and associated tools

## Requirements
Install the following via relevant package manager (I am assuming pacman or Yay as I use arch, btw)
- git `$ pacman -Sy git`
- stow `$ pacman -Sy stow`
- yet another yogurt (yay)
```
$ git clone https://aur.archlinux.org/yay-bin.git
$ cd yay-bin
$ makepkg -si
```

## Installation

Clone the repo via git into $HOME directory and execute stow to create the necessary symlinks.

```
$ git clone git@github.com/jacobsky/dotfiles.git
$ cd dotfiles
$ stow .
```

## Arch CFG

To help track the "generally used arch packages" that I use, I am maintaining both the `pacman` and `yay` (AUR) packages.

Install them as per
```
pacman -S - < ./archcfg/package_list.txt
yay -S - < ./archcfg/aur_package_list.txt
```

Backing up any additionally installed packages can be done using the `scripts/archpacbu.sh` script and running it from you home `~` directory.
