#!/bin/bash
sudo pacman -Syu - </home/$USER/dotfiles/archcfg/package_list.txt
sudo yay -Syu - <pacman >/home/$USER/dotfiles/archcfg/package_list_aur.txt
