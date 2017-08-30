<?php
/*

        studio-ini.php
        Get or set remote BNO055 sensor readings.
        This page services the write requests from sensors and read
        requests from PLC client.
*/

// Parse action, sensor and payload as required

$action = (isset($_GET['action']) ? $_GET['action'] : null);
$sensor = $_GET['sensor'];
if ($action == "r") {
        $cmd = "tail -1 /tmp/" . $sensor . "log.txt";
        $cmdout = shell_exec($cmd);
        echo $cmdout;
} elseif ($action == "w") {
        $payload = (isset($_GET['payload']) ? $_GET['payload'] : null);
        $cmd = "echo \"$payload\" >> /tmp/" . $sensor . "log.txt";
        // echo $cmd;
        shell_exec($cmd);
} else {
        echo "Malformed URL - action=(w|r) required.";
}

?>
