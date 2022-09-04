import * as ws from 'ws';
import { v4 } from 'uuid';
import * as fs from 'fs';

const server = new ws.WebSocketServer({ port: 3002 });

const leaderboard = [];

if (!fs.existsSync('leaderboard.json')) {
    fs.writeFileSync('./leaderboard.json', '[]');
}

fs.readFileSync('./leaderboard.json', 'utf8', (err, data) => {
    if (err) {
        console.log(err);
    } else {
        leaderboard.push(...JSON.parse(data));
    }
});

const clients = [];
let count = 0;

function broadcast(data) {
    server.clients.forEach(client => {
        client.send(data);
    });
}

function buildRealtimeLeaderboardPacket() {
    const leaderboard = clients.map(client => {
        return {
            i: client.id,
            h: client.height
        };
    });

    leaderboard.sort((a, b) => {
        return b.height - a.height;
    });

    return JSON.stringify({
        t: 'li',
        p: JSON.stringify({
            l: leaderboard
        })
    });
}

function buildJoinPacket(client) {
    return JSON.stringify({
        t: 'j',
        p: JSON.stringify({
            i: client.id,
            n: client.name,
            c: client.color
        })
    });
}

function buildLeavePacket(client) {
    return JSON.stringify({
        t: 'l',
        p: JSON.stringify({
            i: client.id
        })
    });
}

function buildLeaderboardPacket() {
    return JSON.stringify({
        t: 'lb',
        p: JSON.stringify({
            l: leaderboard
        })
    });
}

function buildLeaderboardEntry(name, height) {
    return {
        n: name,
        h: height
    };
}

server.on('listening', () => {
    console.log('Server is listening on port 3002');
});

server.on('connection', socket => {
    socket.id = count++;

    setInterval(() => {
        socket.ping();
    }, 5000);

    socket.send(JSON.stringify({
        t: 'i',
        p: socket.id
    }));

    socket.on('message', message => {
        const data = JSON.parse(message);

        switch (data.t) {
            case 'n': {
                if (clients.find(client => client.id === socket.id)) {
                    return;
                }

                socket.name = data.p;
                socket.height = 0;
                socket.color = '#' + Math.floor(Math.random() * 16777215).toString(16);

                clients.forEach(client => {
                    socket.send(buildJoinPacket(client));
                });

                clients.push(socket);
                broadcast(buildJoinPacket(socket));

                break;
            }
            case 'h': {
                socket.height = data.p;
                broadcast(buildRealtimeLeaderboardPacket());
                break;
            }
            case 'pl': {
                const tempValue = leaderboard.find(entry => entry.n === data.p.n);
                if (tempValue) {
                    if (data.p.h > tempValue.h) {
                        leaderboard.find(entry => entry.n === data.p.n).h = data.p.h;
                        fs.writeFileSync('./leaderboard.json', JSON.stringify(leaderboard));
                    }
                }
                else
                {
                    leaderboard.push(buildLeaderboardEntry(data.p.n, data.p.h));
                    fs.writeFileSync('./leaderboard.json', JSON.stringify(leaderboard));
                }
                socket.send(buildLeaderboardPacket());
                break;
            }
            case 'gl': {
                socket.send(buildLeaderboardPacket());
                break;
            }
        }
    });

    socket.on('close', () => {
        clients.splice(clients.indexOf(socket), 1);
        broadcast(buildLeavePacket(socket));
    });
});
