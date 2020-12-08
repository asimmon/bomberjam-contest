import zipfile
import os
import shutil


def unpack(file_path, delete_archive_on_success=True, destination_path=None):
    """Unpacks and deletes a zip file into the files current path"""
    if destination_path is None:
        destination_path = os.path.dirname(file_path)

    with zipfile.ZipFile(file_path) as f:
        f.extractall(destination_path)

    # Remove __MACOSX folder if present
    mac_folder_path = os.path.join(destination_path, "__MACOSX")
    if os.path.exists(mac_folder_path) and os.path.isdir(mac_folder_path):
        shutil.rmtree(mac_folder_path)

    if delete_archive_on_success:
        os.remove(file_path)


def pack(folder_path, destination_file_path):
    """Zips a folder to a path"""
    zip_file = zipfile.ZipFile(destination_file_path, "w", zipfile.ZIP_DEFLATED)

    original_dir = os.getcwd()
    try:
        os.chdir(folder_path)

        for rootPath, dirs, files in os.walk("."):
            for file in files:
                if os.path.basename(file) != os.path.basename(destination_file_path):
                    zip_file.write(os.path.join(rootPath, file))

        zip_file.close()
    finally:
        os.chdir(original_dir)
