<?php
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    // Collect form data
    $name = filter_var($_POST['name'], FILTER_SANITIZE_STRING);
    $email = filter_var($_POST['email'], FILTER_SANITIZE_EMAIL);
    $phone = filter_var($_POST['phone'], FILTER_SANITIZE_STRING);
    $message = filter_var($_POST['message'], FILTER_SANITIZE_STRING);

    // Prepare data to be written to a file
    $data = "Name: $name\nEmail: $email\nPhone: $phone\nMessage: $message\n\n";

    // Write the data to a local file
    $file = 'form_submissions.txt'; // Create or open a file on the server
    file_put_contents($file, $data, FILE_APPEND | LOCK_EX);

    // Output success message
    echo "Form submission saved successfully.";
}
?>
