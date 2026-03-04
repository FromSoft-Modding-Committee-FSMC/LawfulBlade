<?php
header("Content-type:application/json");

// Create our response...
$response = array(
    'Version' => "103",
    'SourceF' => "https://lawful.swordofmoonlight.com/V103.zip"
);

// Convert to json and echo out
$response_json = json_encode($response);
echo $response_json;
?>