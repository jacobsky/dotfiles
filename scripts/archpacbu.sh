#!/bin/bash
pacman -Qqen >/home/$USER/dotfiles/archcfg/package_list.txt
pacman -Qqem >/home/$USER/dotfiles/archcfg/package_list_aur.txt
