% plot_signals.m
% Grafica señales medidas vs deseadas exportadas desde RobotClient.
%
% Uso:
%   plot_signals            → abre diálogo para seleccionar el CSV
%   plot_signals('ruta')    → carga el CSV en la ruta indicada
%
% Columnas del CSV (generado por GeomagicPlotsControl.ExportToCsv):
%   t                          tiempo [s]
%   q1..q3, x..z               posición medida [°] / [cm]
%   dq1..dq3, ddq1..ddq3       velocidad [°/s] y aceleración [°/s²] medidas
%   q1d..q3d, xd..zd           posición deseada
%   dq1d..dq3d, ddq1d..ddq3d   velocidad y aceleración deseadas

function plot_signals(csv_path)

%% ── Cargar datos ─────────────────────────────────────────────────────────
if nargin < 1 || isempty(csv_path)
    [file, folder] = uigetfile('*.csv', 'Seleccionar archivo de datos');
    if isequal(file, 0), return; end
    csv_path = fullfile(folder, file);
end

T = readtable(csv_path, 'TreatAsEmpty', {'NaN'});
t = T.t;

%% ── Colores y estilo ─────────────────────────────────────────────────────
cMeas  = [0.27  0.51  0.71];   % azul acero  (medida)
cDes   = [0.93  0.27  0.13];   % naranja-rojo (deseada)
lwMeas = 1.5;
lwDes  = 1.2;

%% ── Posición articular ───────────────────────────────────────────────────
fig1 = figure('Name', 'Posición Articular', 'NumberTitle', 'off', ...
              'Color', 'w', 'Units', 'normalized', 'Position', [0.05 0.55 0.40 0.40]);

ax_pos = gobjects(3,1);
for k = 1:3
    ax_pos(k) = subplot(3, 1, k);
    plot(t, T.(['q' num2str(k)]),        'Color', cMeas, 'LineWidth', lwMeas); hold on;
    plot(t, T.(['q' num2str(k) 'd']),    'Color', cDes,  'LineWidth', lwDes, 'LineStyle', '--');
    ylabel(['q_' num2str(k) ' [°]']);
    grid on; box off;
    if k == 1
        legend('Medida', 'Deseada', 'Location', 'best', 'FontSize', 8);
    end
end
xlabel(ax_pos(3), 't [s]');
linkaxes(ax_pos, 'x');
sgtitle('Posición Articular', 'FontWeight', 'bold');

%% ── Velocidad articular ──────────────────────────────────────────────────
figure('Name', 'Velocidad Articular', 'NumberTitle', 'off', ...
       'Color', 'w', 'Units', 'normalized', 'Position', [0.50 0.55 0.40 0.40]);

ax_vel = gobjects(3,1);
for k = 1:3
    ax_vel(k) = subplot(3, 1, k);
    plot(t, T.(['dq' num2str(k)]),       'Color', cMeas, 'LineWidth', lwMeas); hold on;
    plot(t, T.(['dq' num2str(k) 'd']),   'Color', cDes,  'LineWidth', lwDes, 'LineStyle', '--');
    ylabel(['\dot{q}_' num2str(k) ' [°/s]']);
    grid on; box off;
    if k == 1
        legend('Medida', 'Deseada', 'Location', 'best', 'FontSize', 8);
    end
end
xlabel(ax_vel(3), 't [s]');
linkaxes(ax_vel, 'x');
sgtitle('Velocidad Articular', 'FontWeight', 'bold');

%% ── Aceleración articular ────────────────────────────────────────────────
figure('Name', 'Aceleración Articular', 'NumberTitle', 'off', ...
       'Color', 'w', 'Units', 'normalized', 'Position', [0.05 0.05 0.40 0.40]);

ax_acc = gobjects(3,1);
for k = 1:3
    ax_acc(k) = subplot(3, 1, k);
    plot(t, T.(['ddq' num2str(k)]),      'Color', cMeas, 'LineWidth', lwMeas); hold on;
    plot(t, T.(['ddq' num2str(k) 'd']),  'Color', cDes,  'LineWidth', lwDes, 'LineStyle', '--');
    ylabel(['\ddot{q}_' num2str(k) ' [°/s²]']);
    grid on; box off;
    if k == 1
        legend('Medida', 'Deseada', 'Location', 'best', 'FontSize', 8);
    end
end
xlabel(ax_acc(3), 't [s]');
linkaxes(ax_acc, 'x');
sgtitle('Aceleración Articular', 'FontWeight', 'bold');

%% ── Posición cartesiana ──────────────────────────────────────────────────
figure('Name', 'Posición Cartesiana', 'NumberTitle', 'off', ...
       'Color', 'w', 'Units', 'normalized', 'Position', [0.50 0.05 0.40 0.40]);

ejes   = {'x', 'y', 'z'};
ax_xyz = gobjects(3,1);
for k = 1:3
    ax_xyz(k) = subplot(3, 1, k);
    plot(t, T.(ejes{k}),                 'Color', cMeas, 'LineWidth', lwMeas); hold on;
    plot(t, T.([ejes{k} 'd']),           'Color', cDes,  'LineWidth', lwDes, 'LineStyle', '--');
    ylabel([ejes{k} ' [cm]']);
    grid on; box off;
    if k == 1
        legend('Medida', 'Deseada', 'Location', 'best', 'FontSize', 8);
    end
end
xlabel(ax_xyz(3), 't [s]');
linkaxes(ax_xyz, 'x');
sgtitle('Posición Cartesiana', 'FontWeight', 'bold');

end
