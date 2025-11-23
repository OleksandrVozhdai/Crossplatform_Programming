# -*- mode: ruby -*-
# vi: set ft=ruby :

Vagrant.configure("2") do |config|
  config.vm.box = "ubuntu/focal64"
  config.vm.network "forwarded_port", guest: 22, host: 2222, id: "ssh"
  config.vm.network "forwarded_port", guest: 5052, host: 5052
  config.vm.network "forwarded_port", guest: 7147, host: 7147

  # Синхронізуємо папку з Windows, де лежить .nupkg
  config.vm.synced_folder "C:/cp/Crossplatform_Programming", "/home/vagrant/app"

  # Provision script
  config.vm.provision "shell", inline: <<-SHELL
    # Оновлення системи
    sudo apt-get update -y

    # Встановлення wget, curl та unzip, якщо нема
    sudo apt-get install -y wget curl unzip

    # Встановлення .NET 8 SDK
    if ! command -v dotnet &> /dev/null
    then
        wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
        bash dotnet-install.sh --channel 8.0
        echo 'export PATH=$PATH:$HOME/.dotnet' >> ~/.bashrc
        export PATH=$PATH:$HOME/.dotnet
    fi

    # Додаємо локальний NuGet репозиторій
    mkdir -p /home/vagrant/nuget-local
    dotnet nuget add source /home/vagrant/nuget-local --name LocalRepo

    # Створюємо тестовий проект і додаємо пакет
    cd /home/vagrant
    if [ ! -d "DiseaseOutbreakTest" ]; then
        dotnet new console -o DiseaseOutbreakTest
    fi
    cd DiseaseOutbreakTest

    dotnet add package disease-outbreaks-detector.Id --version 1.0.0 --source /home/vagrant/nuget-local
    dotnet restore
    dotnet build
  SHELL
end
